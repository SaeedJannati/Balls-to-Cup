using System.Collections.Generic;
using System.Linq;
using BallsToCup.General;
using BallsToCup.Meta.Levels;
using UnityEngine;

namespace BallsToCup.Core
{
    public class TubeView : MonoBehaviour, IEventListener
    {
        #region Fields

        [field: SerializeField] public Transform tubePivot { get; private set; }
        [SerializeField] private Transform tubeParent;
        private Rigidbody _rigidbody;
        private TubeEventController _eventController;
        private float _deltaAngle;
        private float _maxRotVelocity;
        private Vector3 _boundsMin;
        private Vector3 _boundsMax;
        #endregion

        #region Unity actions

        private void Awake()
        {
            TryGetComponent(out _rigidbody);
        }

    

        private void OnDestroy()
        {
            UnregisterFromEvents();
        }

        private void FixedUpdate()
        {
            var deltaTime = Time.fixedDeltaTime;
            var velocity = _deltaAngle / deltaTime;

            if (velocity * velocity > _maxRotVelocity * _maxRotVelocity)
            {
                var sign = velocity > 0 ? 1 : -1;
                _deltaAngle = _maxRotVelocity * deltaTime * sign;
            }

            var deltaRot =
                Quaternion.Euler(_deltaAngle * Vector3.forward);
            var destRot = _rigidbody.rotation * deltaRot;
            _rigidbody.MoveRotation(destRot);
            _deltaAngle = 0.0f;
        }

        #endregion

        #region Methods

        public void Initialise()
        {
            RegisterToEvents();
            GetRenderersBounds();
        }


        public void SetTube(TubeComposite tube)
        {
            var tubeTransform = tube.gameObject.transform;
            tubeTransform.SetParent(tubeParent);
            tubeTransform.localPosition = Vector3.zero;
        }

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

        void GetRenderersBounds()
        {
            var bounds = GetComponentsInChildren<MeshRenderer>()?.Select(i=>i.bounds);
            if(bounds==default)
                return;
            _boundsMax.x = bounds.Max(i => i.max.x);
            _boundsMax.y= bounds.Max(i => i.max.y);
            _boundsMax.z=bounds.Max(i => i.max.z);
            _boundsMin.x = bounds.Min(i => i.min.x);
            _boundsMin.y= bounds.Min(i => i.min.y);
            _boundsMin.z=bounds.Min(i => i.min.z);
        }

        private (Vector3 min, Vector3 max) OnTubeBoundsRequest()
        {
            return (_boundsMin, _boundsMax);
        }

        #endregion
    }
}