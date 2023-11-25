using System;
using Zenject;

namespace BallsToCup.Core.Gameplay
{
    public class LevelManager : IDisposable, IInitializable
    {
        #region Fields

        [Inject] private LevelManagerModel _model;
        [Inject] private LevelManagerEventController _eventController;
        [Inject] private FlowControllerEventController _flowEventController;

        #endregion

        #region Methods

        public void Dispose()
        {
            _eventController.Dispose();
            GC.SuppressFinalize(this);
        }

        public void Initialize()
        {
        }

        private void RegisterToEvents()
        {
            
        }

        private void UnregisterFromEvents()
        {
            
        }

        #endregion
    }
}