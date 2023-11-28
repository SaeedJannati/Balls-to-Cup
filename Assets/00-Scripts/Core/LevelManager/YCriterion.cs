using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallsToCup.Core.Gameplay
{
    public class YCriterion : MonoBehaviour
    {
        #region Fields

        private Transform _transform;

        #endregion

        #region Unity actions

        private void Awake()
        {
            _transform = transform;
        }

        #endregion
        #region Properties

        public float GetYCriterion => _transform.position.y;


        #endregion
    }

}
