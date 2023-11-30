using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using BallsToCup.Core.UI;
using BallsToCup.General;
using BallsToCup.General.Popups;
using UnityEngine;
using UnityEngine.AddressableAssets;
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
        [Inject] private CoroutineHelper _coroutineHelper;
        [Inject] private FlowControllerModel _model;
        [Inject] private AddressableLoader _addressableLoader;
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
            _coroutineHelper.StartCoroutine(CreateWinVFX());
        }
        IEnumerator CreateWinVFX()
        {
            var req = Addressables
                .LoadAssetAsync<GameObject>(_model.winGameVfx);
            yield return req.WaitForCompletion();
            if (req.Result == default)
                throw new Exception("No Such asset exists!");
            if (!req.Result.TryGetComponent(out CoreEndGameEffect vfxPrefab))
            {
                _model.winGameVfx.ReleaseAsset();
                throw new Exception("No Such asset exists!");
            }

            _model.winGameVfx.ReleaseAsset();
            GameObject.Instantiate(vfxPrefab, Camera.main.transform).ShowEffect();
            yield return new WaitForSeconds(1.0f);
        }
        private  void OnLevelGenerationComplete()
        {
            _eventController.onEnableInput.Trigger(true);
            _eventController.onGameStart.Trigger();
        }

        #endregion
    }
}