using System.Collections.Generic;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace BallsToCup.Core.Gameplay
{
    public class LevelManagerModel:ScriptableObject
    {
        #region Properties

        [field: SerializeField, Expandable] public List<BallsToCupLevel> levels { get; private set; } = new();


        #endregion

        #region Methods

        private void OnValidate()
        {
#if UNITY_EDITOR
            for (int i = 0,e=levels.Count ;i < e; i++)
            {
                levels[i].SetIndex(i);
            }
            EditorUtility.SetDirty(this);
#endif
        }

        #endregion
    }
}