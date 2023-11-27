using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace BallsToCup.Core.UI
{
    public class CorePauseMenu : MonoBehaviour
    {
        #region Fields

        [SerializeField, Expandable] private CorePauseMenuModel _model;
        [SerializeField] private Image _button_audio;
        [SerializeField] private Image _button_music;

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
        }

        public void OnHomeClick()
        {
        }

        public void OnRetryClick()
        {
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
        }

        #endregion
    }
}