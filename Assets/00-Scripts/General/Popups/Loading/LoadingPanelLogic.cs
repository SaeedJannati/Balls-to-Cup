using System;
using System.Collections;
using System.Collections.Generic;
using BallsToCup.General;
using BallsToCup.General.Popups;
using UnityEngine;
using Zenject;

namespace BallsToCup.Meta.Popups
{
    public class LoadingPanelLogic : IPopupLogic, IEventListener,IDisposable
    {
        #region Fields

        [Inject] private LoadingPanelEventController _eventController;
        private LoadingPanelView _view;

        #endregion

        #region Properties
        public bool _hasDisposed { get; private set; } = false;
        #endregion
        #region Constructors

        public LoadingPanelLogic(IPopupView view)
        {
            _view = (LoadingPanelView)view;
        }

        #endregion

        #region Methods

        [Inject]
        void Initialise()
        {
            _view.SetEventController(_eventController);
            RegisterToEvents();
        }

        public GameObject GetPanelObject()
        {
            return _view.gameObject;
        }

        public void Close()
        {
        }

        public void Hide()
        {
            ((IPopupLogic)this).OnExit(_view.canvasGroup, onComplete: () => { _view.gameObject.SetActive(false); });
        }

        public void Show()
        {
            _view.gameObject.SetActive(true);
            ((IPopupLogic)this).OnEnter(_view.canvasGroup);
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
        public void Dispose()
        {
            _hasDisposed = true;
            _eventController?.Dispose();
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Factory

        public class Factory : PlaceholderFactory<IPopupView, LoadingPanelLogic>
        {
        }


        #endregion
    }
}
