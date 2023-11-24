using UnityEngine;

namespace BallsToCup.Core
{
    public class TubeRotatorModel:ScriptableObject
    {
        #region Propeties

        [field: SerializeField] public float rotationSpeed { get; private set; } = 1.0f;
        [field: SerializeField] public float deltaPointerPosTreshhold { get; private set; } = .1f;


        #endregion
    }
}