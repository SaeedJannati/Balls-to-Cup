using UnityEngine;
using Zenject;

namespace BallsToCup.Core.Gameplay
{
    public class CupEdgeHandler : MonoBehaviour
    {
        #region Fields

        [Inject] private GameManagerEventController _gameManagerEventController;


        #endregion

        #region Unity actions

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

