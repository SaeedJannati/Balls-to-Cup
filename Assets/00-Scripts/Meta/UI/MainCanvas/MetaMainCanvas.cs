using System.Collections;
using System.Collections.Generic;
using BallsToCup.General;
using UnityEngine;

namespace BallsToCup.Meta.UI
{
    public class MetaMainCanvas : MonoBehaviour
    {
        #region Fields

        [SerializeField] private MetaSelectLevelPanel _selectLevelPanel;
        

        #endregion
        #region Methods

        public void OnPlayClick()
        {
            _selectLevelPanel.BringUp();
        }

        #endregion
    }
}

