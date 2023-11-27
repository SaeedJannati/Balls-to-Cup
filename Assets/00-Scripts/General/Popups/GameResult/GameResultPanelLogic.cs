using System;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace BallsToCup.General.Popups
{
    public class GameResultPanelLogic: IPopupLogic, IEventListener, IDisposable
    {
        #region Fields

        private GameResultPanelView _view;
        [Inject] private GameResultPanelEventController _eventController;
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
                .SetEventController(_eventController);
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
        }

        public void UnregisterFromEvents()
        {
            _eventController.onDispose.Remove(OnViewDestroy);
        }

        private void OnViewDestroy()
        {
            UnregisterFromEvents();
            Dispose();
        }



        private void OnCloseClicked()
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
        #endregion

        #region Factory

        public class Factory : PlaceholderFactory<IPopupView,GameResultPanelLogic>
        {
            
        }

        #endregion


      
    }
}