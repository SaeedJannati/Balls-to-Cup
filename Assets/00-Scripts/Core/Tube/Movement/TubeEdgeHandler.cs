using System;
using System.Collections;
using System.Collections.Generic;
using BallsToCup.General;
using UnityEngine;

namespace BallsToCup.Core.Gameplay
{
    public class TubeEdgeHandler : MonoBehaviour
    {
        #region Fields

        public readonly BaseEventController.SimpleEvent<bool> onBallTrigger = new();

        #endregion

        #region unity actions

        private void OnTriggerExit(Collider other)
        {
            if(!other.TryGetComponent(out CoreBallView view))
                return;
            var isGettingOut = Vector3.Dot(view.ballRigidBody.velocity, transform.up) > 0;
            onBallTrigger.Trigger(isGettingOut);
        }

        #endregion
        #region Methods



        #endregion
    }
}

