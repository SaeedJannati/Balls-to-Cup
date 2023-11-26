using System;
using BallsToCup.General;
using Zenject;

namespace BallsToCup.Core.Gameplay
{
    public class GameManager : IInitializable, IDisposable, IEventListener
    {
        #region Fields

        [Inject] private GameManagerEventController _eventController;
        [Inject] private LevelManagerEventController _levelManagerEventController;
        private int _totalBallsCreated;
        private int _ballsOutOfTube;
        private int _ballsInTheCup;

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

        public void RegisterToEvents()
        {
            _eventController.onBallCreated.Add(OnBallCreated);
            _eventController.onBallGotOutOfTube.Add(OnBallGotOutOfTube);
            _eventController.onBallTriggerdCupEdge.Add(OnBallTriggeredCupEdge);
         
        }

        public void UnregisterFromEvents()
        {
            _eventController.onBallCreated.Remove(OnBallCreated);
            _eventController.onBallGotOutOfTube.Remove(OnBallGotOutOfTube);
            _eventController.onBallTriggerdCupEdge.Remove(OnBallTriggeredCupEdge);
        }

        private void OnBallTriggeredCupEdge(bool isGettingIn)
        {
            var delta = isGettingIn ? 1 : -1;
            _ballsInTheCup += delta;
            _eventController.onBallsInCupChange.Trigger(_ballsInTheCup);
        }

        private void OnBallGotOutOfTube(bool isGettingIn)
        {
            var delta = isGettingIn ? 1 : -1;
            _ballsOutOfTube += delta;
        }

        private void OnBallCreated()
        {
            _totalBallsCreated++;
            _eventController.onTotalBallsChange.Trigger(_totalBallsCreated);
        }

        #endregion
    }
}