using System;
using System.Collections;
using System.Collections.Generic;
using BallsToCup.Core;
using BallsToCup.Core.Gameplay;
using BallsToCup.General;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using Zenject;

namespace BallsToCup.Core
{
    public class TubeView : MonoBehaviour, IEventListener
    {
        #region Fields

        [field: SerializeField] public Transform tubePivot { get; private set; }
        private Rigidbody _rigidbody;
        [SerializeField] private MeshRenderer _meshRenderer;
        private TubeEventController _eventController;
        private float _deltaAngle;
        private float _maxRotVelocity;

        #endregion

        #region Unity actions
        private void Awake()
        {
            TryGetComponent(out _rigidbody);
        }
        private void Start()
        {
            RegisterToEvents();
        }

        private void OnDestroy()
        {
            UnregisterFromEvents();
        }

        private void FixedUpdate()
        {
            var deltaTime = Time.fixedDeltaTime;
            var velocity = _deltaAngle / deltaTime;
            
            if (velocity*velocity > _maxRotVelocity*_maxRotVelocity)
            {
                var sign = velocity > 0 ? 1 : -1;
                _deltaAngle = _maxRotVelocity * deltaTime*sign;
            }

            var deltaRot =
                Quaternion.Euler(_deltaAngle * Vector3.forward);
            var destRot = _rigidbody.rotation * deltaRot;
            _rigidbody.MoveRotation(destRot);
            _deltaAngle = 0.0f;
        }

        #endregion

        #region Methods

        public void AddToDeltaAngle(float deltaAngle)
        {
            _deltaAngle += deltaAngle;
        }

        public TubeView SetEventController(TubeEventController eventController)
        {
            _eventController = eventController;
            return this;
        }

        public TubeView SetMaxRotVelocity(float maxVelocity)
        {
            _maxRotVelocity = maxVelocity;
            return this;
        }

        public void RegisterToEvents()
        {
            _eventController.onTubeBoundsRequest.Add(OnTubeBoundsRequest);
        }

        public void UnregisterFromEvents()
        {
            _eventController.onTubeBoundsRequest.Remove(OnTubeBoundsRequest);
        }

        private (Vector3 min, Vector3 max) OnTubeBoundsRequest()
        {
            return (_meshRenderer.bounds.min, _meshRenderer.bounds.max);
        }

        #endregion
    }
}