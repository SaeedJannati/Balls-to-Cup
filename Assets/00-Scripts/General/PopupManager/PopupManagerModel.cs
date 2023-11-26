using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace BallsToCup.General.Popups
{

    public class PopupManagerModel : ScriptableObject
    {
        #region Fields

        public List<PopUpInfo> popUpInfos;

        #endregion

        #region Monobehaviour callbacks

        private void OnValidate()
        {
            foreach (var info in popUpInfos)
            {
                info.OnValidate();
            }
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        #endregion
    }

    [Serializable]
    public class PopUpInfo
    {
        #region Fields

        [SerializeField, HideInInspector] private string name;
        public PopupName popupName;
        public string path;

        #endregion

        #region Methods

        internal void OnValidate()
        {
            name = popupName.ToString();
        }

        #endregion
    }


    [Serializable]
    public enum PopupName
    {
        None,
        MessageBox,
        Loading,
        GameResult,
        Settings,
        End
    }
}