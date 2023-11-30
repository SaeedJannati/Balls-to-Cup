using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace BallsToCup.General
{
    public class GeneralSettingsModel : ScriptableObject
    {
        public AudioSettings audioSettings;

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
                return;
            audioSettings.musicEnable = true;
            audioSettings.sfxEnable = true;
            EditorUtility.SetDirty(this);
#endif
        }
    }

    [Serializable]
    public class AudioSettings
    {
        public bool sfxEnable = true;
        public bool musicEnable = true;
    }
}