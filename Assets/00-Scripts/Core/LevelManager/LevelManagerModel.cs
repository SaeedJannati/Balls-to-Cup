using System.Collections.Generic;
using NaughtyAttributes;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace BallsToCup.Core.Gameplay
{
    public class LevelManagerModel : ScriptableObject
    {
        #region Properties

        [field: SerializeField, Expandable] public List<BallsToCupLevel> levels { get; private set; } = new();

        #endregion

        #region Unity actions

        private void OnValidate()
        {
#if UNITY_EDITOR
            for (int i = 0, e = levels.Count; i < e; i++)
            {
                levels[i].SetIndex(i);
            }

            EditorUtility.SetDirty(this);
#endif
        }

        #endregion

        #region Methods

        [Sirenix.OdinInspector.Button]
        void CreateNewLevel()
        {
#if UNITY_EDITOR
            BallsToCupLevel newLevel = CreateInstance<BallsToCupLevel>();
            newLevel.SetIndex(levels.Count+1);
            var newLevelName = $"BallsToCupLevel {levels.Count+1}.asset";
            AssetDatabase.CreateAsset(newLevel, $"Assets/04-Models/Core/LevelManger/Levels/{newLevelName}");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = newLevel;
            levels.Add(newLevel);
  
#endif
        }

        #endregion
    }
}