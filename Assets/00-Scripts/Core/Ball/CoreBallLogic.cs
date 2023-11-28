using System;
using BallsToCup.General;
using Zenject;

namespace BallsToCup.Core.Gameplay
{
    public class CoreBallLogic : IDisposable, IEventListener
    {
        #region Fields

        [Inject] private GameManagerEventController _gameManagerEventController;
        private readonly CoreBallView _view;
        #endregion

        #region Constructors

        public CoreBallLogic(CoreBallView view)
        {
            _view = view;
        }

        #endregion

   
        #region Methods

        public void Dispose()
        {
            UnregisterFromEvents();
            GC.SuppressFinalize(this);
        }

        [Inject]
        public void Initialise()
        {
            RegisterToEvents();
            _gameManagerEventController.onBallCreated.Trigger();
        }

        public void RegisterToEvents()
        {
        }

        public void UnregisterFromEvents()
        {
        }

        #endregion

        #region Factory

        public class Factory : PlaceholderFactory<CoreBallView, CoreBallLogic>
        {
        }

        #endregion
    }
}