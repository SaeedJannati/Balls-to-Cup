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
        [SerializeField] private TubeEdgeHandler _edgeHandler;
        private TubeEventController _eventController;
        private float _deltaAngle;
        [SerializeField] private Rigidbody _rigidbody;
        private float _maxRotVelocity;

        #endregion

        #region Unity actions

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
            if (_deltaAngle / deltaTime > _maxRotVelocity)
            {
                _deltaAngle = _maxRotVelocity * deltaTime;
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
            _edgeHandler.onBallTrigger.Add(OnBallTrigger);
        }

        public void UnregisterFromEvents()
        {
            _edgeHandler.onBallTrigger.Remove(OnBallTrigger);
        }

        private void OnBallTrigger(bool isGettingOut)
        {
            _eventController.onBallTriggerEdge.Trigger(isGettingOut);
        }

        #endregion
    }
}