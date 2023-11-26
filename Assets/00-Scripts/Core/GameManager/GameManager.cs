using System;
using System.Linq;
using System.Threading.Tasks;
using BallsToCup.General;
using Zenject;

namespace BallsToCup.Core.Gameplay
{
    public class GameManager : IInitializable, IDisposable, IEventListener
    {
        #region Fields

        [Inject] private GameManagerEventController _eventController;
        [Inject] private LevelManagerEventController _levelManagerEventController;
        [Inject] private FlowControllerEventController _flowControllerEventController;
        private int _totalBallsCreated;
        private int _ballsOutOfTube;
        private int _ballsInTheCup;
        private bool _gameStarted;
        private float _endGameDelay;
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
            _flowControllerEventController.onGameStart.Add(OnGameStart);
            _eventController.onGameEnd.Add(OnGameEnd);
        }

        public void UnregisterFromEvents()
        {
            _eventController.onBallCreated.Remove(OnBallCreated);
            _eventController.onBallGotOutOfTube.Remove(OnBallGotOutOfTube);
            _eventController.onBallTriggerdCupEdge.Remove(OnBallTriggeredCupEdge);
            _flowControllerEventController.onGameStart.Remove(OnGameStart);
            _eventController.onGameEnd.Remove(OnGameEnd);
        }

        private void OnGameEnd()
        {
            var currentLevel=_levelManagerEventController.onCurrentLevelRequest.GetFirstResult();
            if(currentLevel==default)
                return;
            if (_ballsInTheCup < currentLevel.starRatios[0].requiredBalls)
            {
                _eventController.onGameLose.Trigger();
                BtcLogger.Log("YouLost!");
                return;
            }

            var starsCount = currentLevel.starRatios.LastOrDefault(i => i.requiredBalls <= _ballsInTheCup)?.index+1??1;
            BtcLogger.Log($"YouWon!|stars:{starsCount}");
            _eventController.onGameWon.Trigger(starsCount);
        }

        private void OnGameStart()
        {
            _gameStarted = true;
            var currentLevel=_levelManagerEventController.onCurrentLevelRequest.GetFirstResult();
            if(currentLevel==default)
                return;
            _endGameDelay = currentLevel.endGameDelay;
        }

        private void OnBallTriggeredCupEdge(bool isGettingIn)
        {
            var delta = isGettingIn ? 1 : -1;
            _ballsInTheCup += delta;
            _eventController.onBallsInCupChange.Trigger(_ballsInTheCup);
        }

        private void OnBallGotOutOfTube(bool isGettingIn)
        {
            if (!_gameStarted)
                return;
            var delta = isGettingIn ? 1 : -1;
            _ballsOutOfTube += delta;
            CheckForGameEnd();
        }

        void CheckForGameEnd()
        {
            if (_ballsOutOfTube >= _totalBallsCreated)
            {
                OnTriggerGameEnd();
            }
        }

        private async void OnTriggerGameEnd()
        {
            await Task.Delay((int)(1000*_endGameDelay));
            _eventController.onGameEnd.Trigger();
        }

        private void OnBallCreated()
        {
            _totalBallsCreated++;
            _eventController.onTotalBallsChange.Trigger(_totalBallsCreated);
        }

        #endregion
    }
}