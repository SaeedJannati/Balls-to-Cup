using System.Collections;
using System.Collections.Generic;
using BallsToCup.Core.Gameplay;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace BallsToCup.Core.UI
{
    public class CoreMainCanvas : MonoBehaviour
    {
        #region Fields

        [Inject] private LevelManagerEventController _levelManagerEventController;


        #endregion

        #region Methods

        [Button]
        void RequestTubeCreate()
        {
            _levelManagerEventController.onCreateTubeRequest.Trigger();
        }


        #endregion
    }
}

