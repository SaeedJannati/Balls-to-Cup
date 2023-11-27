using System;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace BallsToCup.Core.Gameplay
{
    public class CupEdgeHandler : MonoBehaviour
    {
        #region Fields

        [Inject] private GameManagerEventController _gameManagerEventController;
        [SerializeField] private Transform _cup;
        private Vector3 ShakeVector = new Vector3(.03f, 0, .03f);
        #endregion

        #region Unity actions

        private void OnTriggerEnter(Collider other)
        {
            if(!other.gameObject.TryGetComponent(out CoreBallView view))
                return;
            _cup.DOShakePosition(.1f, ShakeVector, 40);
        }

        private void OnTriggerExit(Collider other)
        {
            if(!other.gameObject.TryGetComponent(out CoreBallView view))
                return;
            var isAdding = Vector3.Dot(view.ballRigidBody.velocity, Vector3.up) < 0 ;
            _gameManagerEventController.onBallTriggerdCupEdge.Trigger(isAdding);
        }

        #endregion
    }
}

