using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using BallsToCup.Core;
using BallsToCup.Core.Gameplay;
using BallsToCup.General;
using UnityEngine;
using Zenject;

namespace MyNamespace
{
    public class CameraSizeFitter : MonoBehaviour, IEventListener
    {
        #region Fields

        private Camera _camera;
        private Transform _tubeTransform;
        private MeshRenderer _tubeRender;
        [SerializeField] private MeshRenderer _cupRenderer;
        [Inject] private TubeEventController _tubeEventController;
        [Inject] private LevelManagerEventController _levelManagerEventController;
        private Vector3 _tubeMin;
        private Vector3 _tubeMax;

        #endregion

        #region Unity actions

        private void Awake()
        {
            TryGetComponent(out _camera);
        }

        private void Start()
        {
            RegisterToEvents();
        }

        private void OnDestroy()
        {
            UnregisterFromEvents();
        }

        private void OnDrawGizmos()
        {
            if(!Application.isPlaying)
                return;
            var pos = Vector3.zero;
            Gizmos.color=Color.red;
            pos.y = GetTopScreenY();
            Gizmos.DrawWireSphere(pos,1.0f);
            Gizmos.color=Color.cyan;
            pos.y = GetBottomScreenY();
            Gizmos.DrawWireSphere(pos,1.0f);
        }

        #endregion

        #region Methods

        public void RegisterToEvents()
        {
            _levelManagerEventController.onTubeCreated.Add(OnTubeCreated);
        }

        public void UnregisterFromEvents()
        {
            _levelManagerEventController.onTubeCreated.Remove(OnTubeCreated);
        }

        private async void OnTubeCreated()
        {
            // await Task.Yield();
            // var boundsInfo = _tubeEventController.onTubeBoundsRequest.GetFirstResult();
            // if(boundsInfo==default)
            //     return;
            // _tubeMax = boundsInfo.max;
            // _tubeMin = boundsInfo.min;
            // SetCameraSizeBaseOnCupAndTube();
        }

        private void SetCameraSizeBaseOnCupAndTube()
        {
       
        }

        float GetTopScreenY()
        {
            return _camera.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, 0.0f)).y;
        }

        float GetBottomScreenY()
        {
            return _camera.ViewportToWorldPoint(Vector3.zero).y;
        }

        #endregion
    }
}