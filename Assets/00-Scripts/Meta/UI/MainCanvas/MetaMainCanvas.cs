using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using BallsToCup.General;
using BallsToCupGeneral.Audio;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Unity.VectorGraphics;
using Unity.VectorGraphics.Editor;
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