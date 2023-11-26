using System.Collections;
using UnityEngine;

namespace BallsToCup.General
{
    public class SimpleRotator : MonoBehaviour
    {
        #region Fields

        [SerializeField] private float _rotationSpeed;
        [SerializeField] private Vector3 _rotationAxis;
        [SerializeField]   private Transform _rotateTransform;

        #endregion

        #region Unity events

        private void Awake()
        {
            if(_rotateTransform!=default)
                return;
            _rotateTransform = transform;
        }

        private void OnEnable()
        {
            StartCoroutine(RotateRoutine());
        }
        #endregion

        #region Methods

        IEnumerator RotateRoutine()
        {
            var deltaRot = 0.0f;
            while (true)
            {
                deltaRot = _rotationSpeed * Time.deltaTime; 
                _rotateTransform.Rotate(_rotationAxis,deltaRot);
                yield return null;
            }
        }
        #endregion
    }
}

