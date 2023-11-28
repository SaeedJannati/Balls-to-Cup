using System;
using System.Collections.Generic;
using BallsToCup.Core.Gameplay;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BallsToCup.General
{
    public class PlayerProgressManagerModel : ScriptableObject
    {
        #region Fields

        [field: SerializeField] public LevelManagerModel _levelManagerModel;
        public PlayerProgressData playerProgressData=new();

        #endregion

        #region Methods


        public string GetRawProgressData()
        {
            try
            {
                return JsonConvert.SerializeObject(playerProgressData);
            }
            catch (Exception e)
            {
                BtcLogger.Log("Couldn't parse progress data!,exception:{e}",BtcLogger.Colours.lightRed);
                return default;
            }
        }

        public void ValidateLevelsCount()
        {
            RemoveExceedingLevels();
            AddLackingLevels();
            CheckIndices();
            playerProgressData.levelsProgress[0].hasReached = true;
        }

        private void CheckIndices()
        {
            for (int i = 0, e = playerProgressData.levelsProgress.Count; i < e; i++)
            {
                if (playerProgressData.levelsProgress[i].index == i)
                    continue;
                playerProgressData.levelsProgress[i].index = i;
            }
        }

        private void RemoveExceedingLevels()
        {
            if (playerProgressData.levelsProgress.Count <= _levelManagerModel.levels.Count)
                return;
            playerProgressData.levelsProgress =
                playerProgressData.levelsProgress.GetRange(0, _levelManagerModel.levels.Count);
        }

        private void AddLackingLevels()
        {
            if (playerProgressData.levelsProgress.Count >= _levelManagerModel.levels.Count)
                return;
            for (int i = playerProgressData.levelsProgress.Count, e = _levelManagerModel.levels.Count; i < e; i++)
            {
                playerProgressData.levelsProgress.Add(new()
                {
                    index = i
                });
            }
        }

        #endregion

        #region Locals classes

        [Serializable]
        public class PlayerProgressData
        {
            public int selectedLevel;
            public List<LevelProgress> levelsProgress=new();
        }

        [Serializable]
        public class LevelProgress
        {
            public int index;
            public bool hasReached;
            public int starsCount;
            [JsonIgnore] public bool passed => starsCount > 0;
        }

        #endregion
    }
}