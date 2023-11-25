using System;
using System.Threading.Tasks;
using Zenject;

namespace BallsToCup.Core.Gameplay
{
    public class FlowController : IInitializable, IDisposable
    {
        #region Fields

        [Inject] private FlowControllerEventController _eventController;
        [Inject] private LevelManagerEventController _levelManagerEventController;

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
        }

        private void UnregisterFromEvents()
        {
        }

        #endregion
    }
}