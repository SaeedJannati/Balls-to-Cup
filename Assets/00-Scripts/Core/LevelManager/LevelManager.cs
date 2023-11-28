using System;
using System.Linq;
using System.Threading.Tasks;
using BallsToCup.General;
using BallsToCup.General.Popups;
using Zenject;

namespace BallsToCup.Core.Gameplay
{
    public class LevelManager : IDisposable, IInitializable
    {
        #region Fields

        [Inject] private PlayerProgressManager _progressManager;
        [Inject] private LevelManagerModel _model;
        [Inject] private LevelManagerEventController _eventController;
        [Inject] private FlowControllerEventController _flowEventController;
        [Inject] private PrefHandler _prefHandler;
        [Inject] private PopupManager _popupManager;
        private int _currentLevelIndex;
        private BallsToCupLevel _currentLevel;
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
            ChooseLevel();
        }

        private void ChooseLevel()
        {
            _currentLevelIndex = _progressManager.GetSelectedLevel();
            _currentLevel = _model.levels.FirstOrDefault(i => i.index == _currentLevelIndex);
            if (_currentLevel == default)
                throw new Exception($"No such level with index {_currentLevelIndex} exists!");
            
        }

        private void RegisterToEvents()
        {
            _eventController.onCurrentLevelRotateControllInfoRequest.Add(OnCurrentLevelRotateControlInfoRequest);
            _eventController.onCurrentLevelRequest.Add(OnCurrentLevelRequest);
            _eventController.onTubeCreated.Add(OnTubeCreated);
            _eventController.onBallsGenerationComplete.Add(OnBallsGenerationComplete);
            
        }

        private void UnregisterFromEvents()
        {
            _eventController.onCurrentLevelRotateControllInfoRequest.Remove(OnCurrentLevelRotateControlInfoRequest);
            _eventController.onCurrentLevelRequest.Remove(OnCurrentLevelRequest);
            _eventController.onTubeCreated.Remove(OnTubeCreated);
            _eventController.onBallsGenerationComplete.Remove(OnBallsGenerationComplete);
        }

        private async void OnBallsGenerationComplete()
        {
            _eventController.onLevelGenerationComplete.Trigger();
            await Task.Delay(1000);
            _popupManager.HideLoading();
        }

        private void OnTubeCreated()
        {
            _eventController.onGenerateBallsRequest.Trigger();
            _popupManager.ShowLoading();
        }

        private BallsToCupLevel OnCurrentLevelRequest() => _currentLevel;

        private (float maxSesivity, float moveThreshold) OnCurrentLevelRotateControlInfoRequest()
        {
            return (_currentLevel.maxControllerSensitivity,_currentLevel.tubeDistanceToGround);
        }

     

        #endregion
    }
}