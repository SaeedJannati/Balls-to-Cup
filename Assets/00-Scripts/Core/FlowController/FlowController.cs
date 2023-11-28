using System;
using System.Threading.Tasks;
using BallsToCup.General;
using BallsToCup.General.Popups;
using Zenject;

namespace BallsToCup.Core.Gameplay
{
    public class FlowController : IInitializable, IDisposable
    {
        #region Fields

        [Inject] private FlowControllerEventController _eventController;
        [Inject] private LevelManagerEventController _levelManagerEventController;
        [Inject] private GameManagerEventController _gameManagerEventController;
        [Inject] private PlayerProgressManager _progressManager;
        [Inject] private PopupManager _popupManager;
        #endregion

        #region Methods

        public void Dispose()
        {
            UnregisterFromEvents();
            _eventController.Dispose();
            GC.SuppressFinalize(this);
        }

        public void Initialize()
        {
            RegisterToEvents();
        }

        [Inject]
        void Initialise()
        {
            CreateLevel();
        }

        private async void CreateLevel()
        {
            await Task.Yield();
            _levelManagerEventController.onCreateTubeRequest.Trigger();
        }

        private void RegisterToEvents()
        {
            _levelManagerEventController.onLevelGenerationComplete.Add(OnLevelGenerationComplete);
            _gameManagerEventController.onGameWon.Add(OnGameWon);
            _gameManagerEventController.onGameLose.Add(OnGameLose);
        }

        private void UnregisterFromEvents()
        {
            _levelManagerEventController.onLevelGenerationComplete.Remove(OnLevelGenerationComplete);
            _gameManagerEventController.onGameWon.Add(OnGameWon);
            _gameManagerEventController.onGameLose.Add(OnGameLose);
        }

        private async void OnGameLose()
        {
            var resultPanel = (GameResultPanelLogic)await _popupManager.RequestPopup(PopupName.GameResult);
            resultPanel
                .SetTitle("Lose")
                .SetMessage("You Lost!")
                .SetNextLevelActive(false)
                .SetStars(0);
        }

        private async void OnGameWon(int starsCount)
        {
            _progressManager.OnLevelWon(starsCount);
           var isLastLevel= _progressManager.IsSelectedLevelLast();
            var resultPanel = (GameResultPanelLogic)await _popupManager.RequestPopup(PopupName.GameResult);
            resultPanel
                .SetTitle("Win")
                .SetMessage("You Won!")
                .SetNextLevelActive(!isLastLevel)
                .SetStars(starsCount);

        }

        private async void OnLevelGenerationComplete()
        {
            _eventController.onEnableInput.Trigger(true);
            _eventController.onGameStart.Trigger();
        }

        #endregion
    }
}