using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using BallsToCup.General;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using Zenject;
using AudioSettings = BallsToCup.General.AudioSettings;

namespace BallsToCupGeneral.Audio
{
    public class AudioHandler :  IDisposable
    {
        #region Fields

        [Inject] private AudioHandlerEventController _eventController;
        [Inject] private GeneralSettingsEventHandler _generalSettingsEventHandler;
        [Inject] private CoroutineHelper _coroutineHelper;

        [Inject] private AudioHandlerModel _model;
        private ObjectPoolPrefab<AudioPlayHandler> _audioPlayers;
        private Dictionary<string, LoadedClipInfo> _loadedClips;
        [Inject] private AudioHandlerDataExtractor _dataExtractor;

        private bool _initialised;

        #endregion

        #region Methods

        [Inject]
       async void Initialise()
        {
            _audioPlayers = new ObjectPoolPrefab<AudioPlayHandler>(_model.audioPlayer);
            RegisterToEvents();
            LoadInitialClips();
            _model.clipLibrary.audioHandler = this;

            await Task.Yield();
            
            SetMusicVolume();
            SetSfxVolume();
            
        }

        public AudioPlayHandler PlayClip(string clipName, bool loop = false)
        {
            var player = _audioPlayers.GetObject();
            _coroutineHelper.StartCoroutine(PlayClipRoutine(player, clipName, loop));
            return player;
        }

        public AudioPlayHandler FadeInPlay(string clipName, float fadeDuration)
        {
            var player = _audioPlayers.GetObject();
            _coroutineHelper.StartCoroutine(FadeInPlayRoutine(player, clipName, fadeDuration));
            return player;
        }
        public void FadeOutStop(AudioPlayHandler  player,string clipName, float fadeDuration)
        {
            // var player = _audioPlayers.GetObject();
            _coroutineHelper.StartCoroutine(FadeOutStopRoutine(player, clipName, fadeDuration));
            // return player;
        }
        
        IEnumerator FadeInPlayRoutine(AudioPlayHandler player, string clipName, float fadeDuration)
        {
            while (!_initialised)
            {
                yield return null;
            }

            var clipData = GetClip(clipName);
            if (clipData.clip == default)
                yield break;
            player.FadeInPlay(clipData.clip, clipData.group, clipData.volume, fadeDuration);
        }
        IEnumerator FadeOutStopRoutine(AudioPlayHandler player, string clipName, float fadeDuration)
        {
            while (!_initialised)
            {
                yield return null;
            }

            var clipData = GetClip(clipName);
            if (clipData.clip == default)
                yield break;
            player.FadeOutStop(clipData.clip, clipData.group, clipData.volume, fadeDuration);
        }
        
        IEnumerator PlayClipRoutine(AudioPlayHandler player, string clipName, bool loop)
        {
            if (!_initialised)
            {
                if (!loop)
                    yield break;
            }

            var clipData = GetClip(clipName);
            var delay = new WaitForSeconds(.5f);
            while (clipData.clip == default)
            {
                clipData = GetClip(clipName);
                yield return delay;
            }

            if (clipData.clip == default)
            {
                player.gameObject.SetActive(false);
                yield break;
            }

            player.Play(clipData.clip, clipData.group, clipData.volume, loop);
        }

        LoadedClipInfo GetClip(string clipName)
        {
            if (!_loadedClips.ContainsKey(clipName))
                return new();
            return _loadedClips[clipName];
        }


        void LoadInitialClips()
        {
            LoadGeneralLibrary();
            LoadSceneClips(SceneManager.GetActiveScene().name);
        }

        bool IsMusicEnable()
        {
            var audioSettings = _generalSettingsEventHandler.onAudioSettingsRequest.GetFirstResult();
            if (audioSettings == default)
            {
                audioSettings = new() { sfxEnable = true, musicEnable = true };
                _generalSettingsEventHandler.onAudioSettingsSaveRequest.Trigger(audioSettings);
                return true;
            }
            return audioSettings.musicEnable;
  
        }

        bool IsSfxEnable()
        {
            var audioSettings = _generalSettingsEventHandler.onAudioSettingsRequest.GetFirstResult();
            if (audioSettings == default)
            {
                audioSettings = new() { sfxEnable = true, musicEnable = true };
                _generalSettingsEventHandler.onAudioSettingsSaveRequest.Trigger(audioSettings);
                return true;
            }
            
            return audioSettings.sfxEnable;

        }


        void UpdateAudioSettings(AudioSettings setting)
        {
            SetSfxVolume();
            SetMusicVolume();
        }

        void SetSfxVolume()
        {
            var volume = IsSfxEnable() ? 0.0f : -80.0f;
            _model.audioMixer.SetFloat(_model.sfxVolumeKey, volume);
        }

        void SetMusicVolume()
        {
            var volume = IsMusicEnable() ? 0.0f : -80.0f;
            _model.audioMixer.SetFloat(_model.musicVolumeKey, volume);
        }

        public void RegisterToEvents()
        {
            SceneManager.activeSceneChanged += ActiveSceneChanged;
            _generalSettingsEventHandler.onAudioSettingsSaveRequest.Add(UpdateAudioSettings);
        }

        public void UnregisterFromEvents()
        {
            SceneManager.activeSceneChanged -= ActiveSceneChanged;
            _generalSettingsEventHandler.onAudioSettingsSaveRequest.Remove(UpdateAudioSettings);
        }

        private void ActiveSceneChanged(Scene previous, Scene next)
        {
            LoadNewSceneAudios(next.name);
            UnloadPreviousAudios(previous.name);
        }

        void UnloadPreviousAudios(string sceneName)
        {
            UnloadSceneClips(sceneName);
            _audioPlayers.ClearPool();
        }

       

        void LoadNewSceneAudios(string sceneName)
        {
            LoadSceneClips(sceneName);
        }

        void UnloadSceneClips(string sceneName)
        {
            var library = _dataExtractor.GetSceneAudioLibrary(sceneName);
            UnloadLibrary(library);
        }

        void LoadGeneralLibrary()
        {
            var library = _dataExtractor.LoadGeneralLibrary();
            LoadLibrary(library);
        }

        void LoadSceneClips(string sceneName)
        {
            var library = _dataExtractor.GetSceneAudioLibrary(sceneName);
            LoadLibrary(library);
        }

        async void LoadLibrary(SceneAudioLibrary library)
        {
            _initialised = false;
            _loadedClips ??= new();

            if (library == default)
            {
                _initialised = true;
                return;
            }

            if (library.clipsData == default)
            {
                _initialised = true;
                return;
            }

            var clipCount = library.clipsData.Count;
            var loadedClipsCount = 0;
            for (var index = 0; index < clipCount; index++)
            {
                var item = library.clipsData[index];
                if (_loadedClips.ContainsKey(item.clipName))
                {
                    loadedClipsCount++;
                    continue;
                }

                var req =
                    Addressables.LoadAssetAsync<AudioClip>(item.clipReference);
                var clipInfo = item;
                req.Completed += _ =>
                {
                    loadedClipsCount++;
                    if (req.Result == default)
                    {
                        _initialised = true;
                        return;
                    }

                    _loadedClips[clipInfo.clipName] = new()
                    {
                        clip = req.Result,
                        volume = clipInfo.volume,
                        group = clipInfo.mixerGroup
                    };
                };
            }

            while (loadedClipsCount < clipCount)
            {
                await Task.Yield();
            }

            _initialised = true;
        }

        void UnloadLibrary(SceneAudioLibrary library)
        {
            if (library == default)
                return;
            if (library.clipsData == default)
                return;
            for (int i = 0, topIndex = library.clipsData.Count; i < topIndex; i++)
            {
                library.clipsData[i].clipReference.ReleaseAsset();
            }
        }

        public void Dispose()
        {
            UnregisterFromEvents();
            _eventController?.Dispose();
            _audioPlayers?.Dispose();
        }

        #endregion
    }

    [Serializable]
    public class LoadedClipInfo
    {
        public AudioClip clip;
        public float volume;
        [JsonIgnore] public AudioMixerGroup group;
    }
}