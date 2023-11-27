using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BallsToCup.General;
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
        [Inject] private PlayerProgressManager _progressManager;
        [Inject] private SceneLoader _sceneLoader;
        #endregion

        #region Methods

        public void OnMusicClick()
        {
        }

        public void OnAudioClick()
        {
        }

        public void OnResumeClick()
        {
            BringDown();
        }

        public void OnHomeClick()
        {
            BtcLogger.Log("OnHomeClick");
            _sceneLoader.LoadScene(2, () => { _sceneLoader.LoadScene(0); });
        }

        public void OnRetryClick()
        {
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