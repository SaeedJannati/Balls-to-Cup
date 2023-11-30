using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Audio;

namespace BallsToCupGeneral.Audio
{
   public class AudioHandlerModel : ScriptableObject
    {
        #region Fileds
        public string musicVolumeKey = "MusicVolume";
        public string sfxVolumeKey = "SFXVolume";
        public AudioPlayHandler audioPlayer;
        public AudioMixer audioMixer;
        [Expandable] public AudioHandlerClipLibrary clipLibrary;

        #endregion

        #region Monobehaviour callbacks

        private void OnValidate()
        {
            Initialise();
        }

        #endregion

        #region Methods

        [Button()]
        void Initialise()
        {
            clipLibrary.Initialise(audioMixer);
        }

        #endregion
    }
}