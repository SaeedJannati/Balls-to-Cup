using System;
using System.Collections.Generic;
using UnityEngine;

namespace BallsToCup.Core.UI
{
    public class CorePauseMenuModel:ScriptableObject
    {
        #region Properties

        [field: SerializeField] public List<ButtonColourInfo> colourInfos { get; private set; }

        #endregion

        #region Local classes

        [Serializable]
        public class ButtonColourInfo
        {
            [field: SerializeField] public bool enable { get; private set; }
            [field: SerializeField] public Color colour { get; private set; } = Color.white;
        }


        #endregion
    }
}