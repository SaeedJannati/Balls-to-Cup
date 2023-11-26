using System;
using System.Linq;
using BallsToCup.General;
using Zenject;

namespace BallsToCup.Core.Gameplay
{
    public class LevelManager : IDisposable, IInitializable
    {
        #region Fields

        [Inject] private LevelManagerModel _model;
        [Inject] private LevelManagerEventController _eventController;
        [Inject] private FlowControllerEventController _flowEventController;
        [Inject] private PrefHandler _prefHandler;
        private int _currentLevelIndex;
        private BallsToCupLevel _currentLevel;
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
            ChooseLevel();
        }

        private void ChooseLevel()
        {
            _currentLevelIndex = _prefHandler.GetPref(PrefKeys.LevelKeys.playerLevelKey, 0);
            _currentLevel = _model.levels.FirstOrDefault(i => i.index == _currentLevelIndex);
            if (_currentLevel == default)
                throw new Exception($"No such level with index {_currentLevelIndex} exists!");
            
        }

        private void RegisterToEvents()
        {
            _eventController.onCurrentLevelRotateControllInfoRequest.Add(OnCurrentLevelRotateControlInfoRequest);
            _eventController.onCurrentLevelRequest.Add(OnCurrentLevelRequest);
            _eventController.onTubeCreated.Add(OnTubeCreated);
            
        }

        private void UnregisterFromEvents()
        {
            _eventController.onCurrentLevelRotateControllInfoRequest.Remove(OnCurrentLevelRotateControlInfoRequest);
            _eventController.onCurrentLevelRequest.Remove(OnCurrentLevelRequest);
            _eventController.onTubeCreated.Remove(OnTubeCreated);
        }

        private void OnTubeCreated()
        {
            _eventController.onGenerateBallsRequest.Trigger();
        }

        private BallsToCupLevel OnCurrentLevelRequest() => _currentLevel;

        private (float sensitivityVelocity, float moveThreshold) OnCurrentLevelRotateControlInfoRequest()
        {
            return (_currentLevel.controllerSensitivity, _currentLevel.tubeDistanceToGround);
        }

     

        #endregion
    }
}