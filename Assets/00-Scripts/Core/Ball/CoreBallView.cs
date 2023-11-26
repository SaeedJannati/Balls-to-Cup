using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallsToCup.Core.Gameplay
{
    public class CoreBallView : MonoBehaviour
    {
      #region Properties

      [field: SerializeField] public Transform ballTransform { get; private set; }
      [field: SerializeField] public Rigidbody ballRigidBody  { get; private set; }
      [field: SerializeField] public MeshRenderer ballRenderer{ get; private set; }

      #endregion
    }
}
