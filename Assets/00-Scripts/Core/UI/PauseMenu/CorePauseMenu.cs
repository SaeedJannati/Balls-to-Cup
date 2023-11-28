using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BallsToCup.General;
using BallsToCupGeneral.Audio;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BallsToCup.Core.UI
{
    public class CorePauseMenu : MonoBehaviour
    {
        #region Fields

        [SerializeField, Expandable] private CorePauseMenuModel _model;
        [SerializeField] private Image _button_audio;
        [SerializeField] private Image _button_music;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private AudioPlayer _clickAudioPalyer;
        [Inject] private PlayerProgressManager _progressManager;
        [Inject] private SceneLoader _sceneLoader;
        [Inject] private GeneralSettingsEventHandler _generalSettingsEventHandler;
        #endregion

        #region unity actions

        private void Start()
        {
            InitialiseAudioSettings();
        }

        #endregion
        #region Methods
 
        void InitialiseAudioSettings()
        {
            var audioSettings = _generalSettingsEventHandler.onAudioSettingsRequest.GetFirstResult();
            var sfxEnable = audioSettings?.sfxEnable ?? true;
            var musicEnable = audioSettings?.musicEnable ?? true;
            _button_music.color = _model.colourInfos.FirstOrDefault(i => i.enable == musicEnable).colour;
            _button_audio.color = _model.colourInfos.FirstOrDefault(i => i.enable == sfxEnable).colour;
        }
      
        public void OnMusicClick()
        {
            _clickAudioPalyer.Play();
            var audioSettings = _generalSettingsEventHandler.onAudioSettingsRequest.GetFirstResult();
            var musicEnable = audioSettings?.musicEnable ?? true;
            musicEnable = !musicEnable;
            _button_music.color = _model.colourInfos.FirstOrDefault(i => i.enable == musicEnable).colour;
            audioSettings.musicEnable = musicEnable;
            _generalSettingsEventHandler.onAudioSettingsSaveRequest.Trigger(audioSettings);
        }

        public void OnAudioClick()
        {
            _clickAudioPalyer.Play();
            var audioSettings = _generalSettingsEventHandler.onAudioSettingsRequest.GetFirstResult();
            var sfxEnable = audioSettings?.sfxEnable ?? true;
            sfxEnable = !sfxEnable;
            _button_audio.color = _model.colourInfos.FirstOrDefault(i => i.enable == sfxEnable).colour;
            audioSettings.sfxEnable = sfxEnable;
            _generalSettingsEventHandler.onAudioSettingsSaveRequest.Trigger(audioSettings);
        }

        public void OnResumeClick()
        {
            _clickAudioPalyer.Play();
            BringDown();
        }

        public void OnHomeClick()
        {
            _clickAudioPalyer.Play();
            _sceneLoader.LoadScene(2, () => { _sceneLoader.LoadScene(0); });
        }

        public void OnRetryClick()
        {
            _clickAudioPalyer.Play();
            _sceneLoader.LoadScene(2, () => { _sceneLoader.LoadScene(1); });
        }

        public CorePauseMenu SetMusicEnable(bool enable)
        {
            var colourInfo = _model.colourInfos.FirstOrDefault(i => i.enable == enable);
            if (colourInfo == default)
                return this;
            _button_music.color = colourInfo.colour;
            return this;
        }

        public CorePauseMenu SetAudioEnable(bool enable)
        {
            var colourInfo = _model.colourInfos.FirstOrDefault(i => i.enable == enable);
            if (colourInfo == default)
                return this;
            _button_audio.color = colourInfo.colour;
            return this;
        }

        public void BringUp()
        {
            gameObject.SetActive(true);
            FadePanel(true);
        }

        void BringDown()
        {
            FadePanel(false, () => { gameObject.SetActive(false);});
        }
        void FadePanel(bool fadeIn, Action onComplete = default)
        {
            var initAlpha = fadeIn ? 0.0f : 1.0f;
            var destAlpha = fadeIn ? 1.0f : 0.0f;
            _canvasGroup.alpha = initAlpha;
            _canvasGroup
                .DOFade(destAlpha, .3f)
                .SetEase(Ease.InOutSine)
                .onComplete = () => { onComplete?.Invoke(); };
        }
        #endregion
    }
}