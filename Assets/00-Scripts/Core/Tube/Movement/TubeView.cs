using System;
using System.Collections;
using System.Collections.Generic;
using BallsToCup.Core;
using BallsToCup.General;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace BallsToCup.Core
{
    public class TubeView : MonoBehaviour
    {
        #region Fields
        [field: SerializeField] public Transform tubePivot { get; private set; }
        private TubeEventController _eventController;
        #endregion

        #region Methods

        public TubeView SetEventController(TubeEventController eventController)
        {
            _eventController = eventController;
            return this;
        }


        #endregion


    
    }
}