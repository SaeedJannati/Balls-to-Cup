using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BallsToCup.General;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace BallsToCupGeneral.Audio
{
    public class AudioPlayer : MonoBehaviour
    {
        #region Fields

        private const string audioHandlerClipLibraryPath =
            "Assets/04-Models/General/AudioSystem/AudioHandlerClipLibrary.asset";

        [SerializeField] AudioHandlerClipLibrary _clipLibrary;
        [SerializeField] bool _isGeneralLibrary = true;
        [SerializeField] private bool _loop = false;
        [SerializeField] private bool _playOnEnable = false;

        [Dropdown(nameof(clipNames)), SerializeField]
        string clipName;

        private AudioPlayHandler _handler;
        [Inject] private AudioHandler _audioHandler;

        #endregion

        #region Properteis

        private List<string> clipNames;

        #endregion

        #region Monobehaviour callbacks

        private void OnEnable()
        {
            StartCoroutine(PlayOnEnableRoutine());
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (_clipLibrary == default)
                _clipLibrary =
                    AssetDatabase.LoadAssetAtPath(audioHandlerClipLibraryPath, typeof(AudioHandlerClipLibrary)) as
                        AudioHandlerClipLibrary;
            InitialiseClipNames();
            name = $"AudioPlayer_{clipName}";
            EditorUtility.SetDirty(this);
#endif
        }

        #endregion

        #region Methods

        IEnumerator PlayOnEnableRoutine()
        {
            yield return null;
            yield return null;
            if (_playOnEnable)
                Play();
        }

        public void Stop()
        {
            if(_handler==default)
                return;
            _handler.Stop();
        }

        public void SetAudioHandler(AudioHandler audioHandler)
        {
            _audioHandler = audioHandler;
        }

        [Sirenix.OdinInspector.Button()]
        public float Play()
        {
            _handler = _audioHandler?.PlayClip(clipName, _loop);
            if (_handler == default)
            {
                return 0.0f;
            }

            return _handler.GetLength();
        }

        public AudioPlayHandler FadeInPlay(float fadeDuration)
        {
            _handler = _audioHandler.FadeInPlay(clipName, fadeDuration);
            return _handler;
        }

        public void FadeOutStop(float fadeDuration)
        {
            _audioHandler.FadeOutStop(_handler,clipName, fadeDuration);
        }
        void InitialiseClipNames()
        {
#if UNITY_EDITOR
            var dataExtractor = new AudioHandlerDataExtractor()
                .SetModel(_clipLibrary);
           
            var sceneName = GetSceneName();
            SceneAudioLibrary library;
            if (_isGeneralLibrary)
            {
                library = dataExtractor.LoadGeneralLibrary();
            }
            else
            {
                library=dataExtractor.GetSceneAudioLibrary(sceneName);
            }
            if (library == default)
                return;
            if (library.clipsData == default)
                return;
            if (library.clipsData.Count == 0)
                clipNames = new();
            clipNames = library.clipsData.Select(i => i.clipName).ToList();
#endif
        }

        string GetSceneName()
        {
            if (_isGeneralLibrary)
            {
                return "General";
            }

          
#if UNITY_EDITOR
            return UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().name;
#endif
            return "General";
        }

        public void SetClipName(string aClipName)
        {
            clipName = aClipName;
        }

        public void SetIsGeneral(bool isGeneral)
        {
            _isGeneralLibrary = isGeneral;
        }



        #endregion

        #region Factory

        public class Factory : PlaceholderFactory<AudioPlayer>
        {
        }

        #endregion
    }
}