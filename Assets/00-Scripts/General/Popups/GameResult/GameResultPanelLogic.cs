using System;
using BallsToCupGeneral.Audio;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace BallsToCup.General.Popups
{
    public class GameResultPanelLogic : IPopupLogic, IEventListener, IDisposable
    {
        #region Fields

        private GameResultPanelView _view;
        [Inject] private GameResultPanelEventController _eventController;
        [Inject] private GameResultPanelModel _model;
        [Inject] private PlayerProgressManager _progressManager;
        [Inject] private SceneLoader _sceneLoader;
        [Inject] private AudioHandler _audioHandler;
        private bool _isClosing = false;

        #endregion

        #region Constructors

        public GameResultPanelLogic(IPopupView view)
        {
            SetView(view);
        }

        #endregion

        #region Methods

        [Inject]
        void Initialise()
        {
            InitialiseView();
            ((IPopupLogic)this).OnEnter(_view.canvasGroup);
            RegisterToEvents();
        }

        void InitialiseView()
        {
            _view
                .SetEventController(_eventController)
                .SetAudioHandler(_audioHandler);
        }

        void SetView(IPopupView view)
        {
            _view = (GameResultPanelView)view;
        }

        public GameObject GetPanelObject()
        {
            return _view.gameObject;
        }

        public void Close()
        {
            GameObject.Destroy(_view.gameObject);
        }

        public void RegisterToEvents()
        {
            _eventController.onDispose.Add(OnViewDestroy);
            _eventController.onHomeClick.Add(OnHomeClick);
            _eventController.onRetryClick.Add(OnRetryClick);
            _eventController.onNextLevelClick.Add(OnNextLevelClick);
        }

        public void UnregisterFromEvents()
        {
            _eventController.onDispose.Remove(OnViewDestroy);
            _eventController.onHomeClick.Remove(OnHomeClick);
            _eventController.onRetryClick.Remove(OnRetryClick);
            _eventController.onNextLevelClick.Remove(OnNextLevelClick);
        }

        private void OnNextLevelClick()
        {
            var nextLevel = _progressManager.GetSelectedLevel();
            _progressManager.OnSelectLevel(nextLevel + 1);
            _sceneLoader.LoadScene(2, () => { _sceneLoader.LoadScene(1); });
        }

        private void OnRetryClick()
        {
            _sceneLoader.LoadScene(2, () => { _sceneLoader.LoadScene(1); });
        }

        private void OnHomeClick()
        {
            _sceneLoader.LoadScene(2, () => { _sceneLoader.LoadScene(0); });
        }

        private void OnViewDestroy()
        {
            UnregisterFromEvents();
            Dispose();
        }

        private void OnClose()
        {
            if (_isClosing)
                return;
            ((IPopupLogic)this).OnExit(_view.canvasGroup, onComplete: Close);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _eventController?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public GameResultPanelLogic SetTitle(string title)
        {
            _view.SetTitle(title);
            return this;
        }

        public GameResultPanelLogic SetMessage(string message)
        {
            _view.SetMessage(message);
            return this;
        }

        public GameResultPanelLogic SetStars(int stars)
        {
            for (int i = 0; i < 3; i++)
            {
                if (i < stars)
                {
                    _view.starsForegrounds[i].gameObject.SetActive(true);
                    continue;
                }

                _view.starsForegrounds[i].gameObject.SetActive(false);
            }

            return this;
        }

        public GameResultPanelLogic SetNextLevelActive(bool enable)
        {
            _view.SetNextButtonEnable(enable);
            return this;
        }

        #endregion

        #region Factory

        public class Factory : PlaceholderFactory<IPopupView, GameResultPanelLogic>
        {
        }

        #endregion
    }
}