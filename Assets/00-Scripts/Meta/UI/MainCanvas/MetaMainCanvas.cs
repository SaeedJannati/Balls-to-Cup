using System.Collections;
using System.Collections.Generic;
using BallsToCup.General;
using BallsToCupGeneral.Audio;
using UnityEngine;

namespace BallsToCup.Meta.UI
{
    public class MetaMainCanvas : MonoBehaviour
    {
        #region Fields
        [SerializeField] private MetaSelectLevelPanel _selectLevelPanel;
        [SerializeField] private AudioPlayer _clickAudio;
        #endregion
        #region Methods

        public void OnPlayClick()
        {
            _clickAudio.Play();
            _selectLevelPanel.BringUp();
        }

        #endregion
    }
}

