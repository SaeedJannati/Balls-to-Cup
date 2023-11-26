using System;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace BallsToCup.General.Popups
{
    public class MessageBoxPanelLogic : IPopupLogic, IEventListener, IDisposable
    {
        #region Fields

        private MessageBoxPanelView _view;
        [Inject] private MessageBoxPanelEventController _eventController;
        private bool _isClosing = false;

        private Action _firstButtonClickAction;
        private Action _secondButtonClickAction;
        Action _onComplete;


        private bool _keepOpenAfterFirstClick;
        private bool _keepOpenAfterSecondClick;

        #endregion

        #region Constructors

        public MessageBoxPanelLogic(IPopupView view)
        {
            SetView(view);
        }

        ~MessageBoxPanelLogic()
        {
            Dispose(false);
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
            EnableSecondButton(false);
            SetFirstButtonText("ok");
        }

        void SetView(IPopupView view)
        {
            _view = (MessageBoxPanelView)view;
        }

        public GameObject GetPanelObject()
        {
            return _view.gameObject;
        }

        public void Close()
        {
            if (_view.gameObject.IsDestroyed())
                return;
            GameObject.Destroy(_view.gameObject);
            _onComplete?.Invoke();
        }

        public void RegisterToEvents()
        {
            _eventController.onFirstButtonClick.Add(OnFirstButtonClick);
            _eventController.onSecondButtonClick.Add(OnSecondButtonClick);
            _eventController.onDispose.Add(OnViewDestroy);
        }

        public void UnregisterFromEvents()
        {
            _eventController.onFirstButtonClick.Remove(OnFirstButtonClick);
            _eventController.onSecondButtonClick.Remove(OnSecondButtonClick);
            _eventController.onDispose.Remove(OnViewDestroy);
        }

        private void OnViewDestroy()
        {
            UnregisterFromEvents();
            Dispose();
        }

        private void OnSecondButtonClick()
        {

            if (_keepOpenAfterSecondClick)
            {
                _secondButtonClickAction?.Invoke();
                return;
            }

            ((IPopupLogic)this).OnExit(_view.canvasGroup, onComplete:
                () =>
                {
                    _secondButtonClickAction?.Invoke();
                    Close();
                });
        }

        private void OnFirstButtonClick()
        {

            if (_keepOpenAfterFirstClick)
            {
                _firstButtonClickAction?.Invoke();
                return;
            }

            ((IPopupLogic)this).OnExit(_view.canvasGroup, onComplete: () =>
            {
                _firstButtonClickAction?.Invoke();
                Close();
            });
        }

        private void OnCloseClicked()
        {
            if (_isClosing)
                return;
            ((IPopupLogic)this).OnExit(_view.canvasGroup, onComplete: Close);
        }

        public MessageBoxPanelLogic SetTitle(string title)
        {
            _view.SetTitle(title);
            return this;
        }

        public MessageBoxPanelLogic SetFirstButtonClickAction(Action action)
        {
            _firstButtonClickAction = action;
            return this;
        }

        public MessageBoxPanelLogic SetSecondButtonClickAction(Action action)
        {
            _secondButtonClickAction = action;
            EnableSecondButton(true);
            return this;
        }

        public MessageBoxPanelLogic SetMessage(string message)
        {
            _view.SetMessage(message);
            return this;
        }

        public MessageBoxPanelLogic SetFirstButtonText(string value)
        {
            _view.SetFirstButtonText(value);
            return this;
        }

        public MessageBoxPanelLogic SetSecondButtonText(string value)
        {
            _view.SetSecondButtonText(value);
            return this;
        }

        public MessageBoxPanelLogic DontCloseAfterFirstButtonClick()
        {
            _keepOpenAfterFirstClick = true;
            return this;
        }

        public MessageBoxPanelLogic DontCloseAfterSecondButtonClick()
        {
            _keepOpenAfterSecondClick = true;
            return this;
        }

    

        public MessageBoxPanelLogic EnableSecondButton(bool enable)
        {
            _view.EnableSecondButton(enable);
            return this;
        }

        public void SetOnCompleteAction(Action action)
        {
            _onComplete = action;
        }

        private void ReleaseUnmanagedResources()
        {
            // TODO release unmanaged resources here
        }

        private void Dispose(bool disposing)
        {
            ReleaseUnmanagedResources();
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

        public class Factory : PlaceholderFactory<IPopupView,MessageBoxPanelLogic>
        {
            
        }

        #endregion


      
    }
}

