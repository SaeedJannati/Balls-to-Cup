using System;
using System.Collections.Generic;
using NaughtyAttributes;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Audio;

namespace BallsToCupGeneral.Audio
{
   public class AudioHandlerClipLibrary : ScriptableObject
    {
        #region Fields
        public List<AudioLibraryInfo> audioLibraryInfos;
        public AudioHandler audioHandler;
        #endregion

   
        #region Methods

        public void Initialise(AudioMixer mixer)
        {
#if UNITY_EDITOR
            if (audioLibraryInfos == default)
                return;
            foreach (var item in audioLibraryInfos)
            {
                item.Initialise(mixer);
            }
#endif
        }
        

        #endregion
    }

    [Serializable]
    public class AudioLibraryInfo
    {
        #region Fields
        public string name;
        [Expandable] public SceneAudioLibrary audioLibrary;
        #endregion
        #region Methods
        public void Initialise(AudioMixer mixer)
        {
#if UNITY_EDITOR
            SetName();
            SetClipNames(mixer);
            EditorUtility.SetDirty(audioLibrary);           
#else
#endif
        }

        void SetName()
        {
            if (audioLibrary.isGeneralLibrary)
            {
                name = "General";
                return;
            }

            name = audioLibrary.sceneName;
        }
        void SetClipNames(AudioMixer mixer)
        {
#if UNITY_EDITOR
            for (int i = 0; i < audioLibrary.clipsData.Count; i++)
            {
                if(audioLibrary.clipsData[i].clipName.Length>0)
                    continue;
                audioLibrary.clipsData[i].clipName = audioLibrary.clipsData[i].clipReference.editorAsset.name;
                audioLibrary.clipsData[i].mixerGroup = mixer.FindMatchingGroups("SFX")[0];
                audioLibrary.clipsData[i].volume = 1.0f;
            }
            EditorUtility.SetDirty(audioLibrary);
#endif
        }
        #endregion
    }
}