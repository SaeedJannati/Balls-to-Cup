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
            _levelManagerEventController.onLevelGenerationComplete.Add(OnLevelGenerationComplete);
        }

        private void UnregisterFromEvents()
        {
            _levelManagerEventController.onLevelGenerationComplete.Remove(OnLevelGenerationComplete);
        }

        private async void OnLevelGenerationComplete()
        {
            await Task.Delay(2000);
            _eventController.onEnableInput.Trigger(true);
            _eventController.onGameStart.Trigger();
        }

        #endregion
    }
}