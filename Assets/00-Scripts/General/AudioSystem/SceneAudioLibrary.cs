
using System;
using System.Collections.Generic;
using BallsToCup.General.Editor;

using NaughtyAttributes;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;

namespace BallsToCupGeneral.Audio
{
  
    public class SceneAudioLibrary : ScriptableObject
    {
        #region Fields

        [InspectorReadOnly] public string sceneName;
        public bool isGeneralLibrary;
#if UNITY_EDITOR
        [HideIf(nameof(isGeneralLibrary))] public SceneAsset scene;
#endif
        public List<ClipData> clipsData;

        #endregion

        #region Methods

        public void OnValidate()
        {
#if UNITY_EDITOR
            if(scene==default)
                return;
            sceneName = scene.name;
            EditorUtility.SetDirty(this);
#endif
        }

        #endregion
    }

    [Serializable]
    public class ClipData
    {
        public string clipName;
        [JsonIgnore] public AssetReference clipReference;
        [JsonIgnore] public AudioMixerGroup mixerGroup;
        [JsonIgnore] [Range(0.0f, 1.0f)] public float volume = 1.0f;
    }
}