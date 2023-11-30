using System;
using System.Collections.Generic;
using Zenject;

namespace BallsToCup.General
{
    public class PlayerProgressManager:IDisposable,IEventListener
    {
        #region Fields

        [Inject] private PlayerProgressManagerModel _model;
        [Inject] private PrefHandler _prefHandler;
        #endregion
        
        #region Methods

        [Inject]
        private void Initialise()
        {
            RegisterToEvents();
        }

        public bool IsSelectedLevelLast()
        {
            LoadPlayerProgress();
            return _model.playerProgressData.selectedLevel == _model.playerProgressData.levelsProgress.Count - 1;
        }

        public void LoadPlayerProgress()
        {
            _model.playerProgressData = _prefHandler.GetPref(PrefKeys.PlayerProgressKeys.playerProgressKey,
                new PlayerProgressManagerModel.PlayerProgressData());
            _model.ValidateLevelsCount();
        }

        public void SavePlayerProgress()
        {
            if (_model.playerProgressData == default)
                return;
            _model.ValidateLevelsCount();
            _prefHandler.SetPref(PrefKeys.PlayerProgressKeys.playerProgressKey,
                _model.playerProgressData);
        }

        public void OnSelectLevel(int levelIndex)
        {
            LoadPlayerProgress();
            _model.playerProgressData.selectedLevel = levelIndex;
            SavePlayerProgress();
        }

        public void SetLastUnlocked(int levelIndex)
        {
            LoadPlayerProgress();
            
            for (int i = 0,e=_model.playerProgressData.levelsProgress.Count; i < e; i++)
            {
                if (i <= levelIndex)
                {
                    _model.playerProgressData.levelsProgress[i].hasReached = true;
                    continue;
                }
                _model.playerProgressData.levelsProgress[i].hasReached = false;
                _model.playerProgressData.levelsProgress[i].starsCount = 0;
            }
            SavePlayerProgress();
        }

        public void OnLevelWon(int stars,int levelIndex=-1 )
        {
            if (levelIndex == -1)
                levelIndex = _model.playerProgressData.selectedLevel;
            var levelProgress = _model.playerProgressData.levelsProgress[levelIndex];
            if (levelProgress.starsCount<stars)
            {
                levelProgress.starsCount = stars;
            }

            _model.playerProgressData.levelsProgress[levelIndex] = levelProgress;
            if (levelProgress.index >= _model.playerProgressData.levelsProgress.Count - 1)
            {
                SavePlayerProgress();
                return;
            }

            _model.playerProgressData.levelsProgress[levelIndex + 1].hasReached = true;
            SavePlayerProgress();


        }

        public int GetSelectedLevel()
        {
            LoadPlayerProgress();
            return _model.playerProgressData.selectedLevel;
        }

        public List<PlayerProgressManagerModel.LevelProgress> GetLevelsProgress()
        {
            LoadPlayerProgress();
            return _model.playerProgressData.levelsProgress;
        }

        public void Dispose()
        {
            UnregisterFromEvents();
            GC.SuppressFinalize(this);
        }
        public void RegisterToEvents()
        {
            SROptions.onUnlockLevelRequest.Add(OnUnlockLevelRequest);
        }

        public void UnregisterFromEvents()
        {
          SROptions.onUnlockLevelRequest.Remove(OnUnlockLevelRequest);
        }

        private void OnUnlockLevelRequest(int level)
        {
            if(level<1)
            {
                BtcLogger.Log($"No such level as {level} exists!",BtcLogger.Colours.lightRed);
            }
            if (level > _model._levelManagerModel.levels.Count )
            {
                BtcLogger.Log($"There are {_model._levelManagerModel.levels.Count} levels therefore no such level as {level} exists!",BtcLogger.Colours.lightRed);
            }

            SetLastUnlocked(level-1);
        }

        #endregion


       
    }
    
}

