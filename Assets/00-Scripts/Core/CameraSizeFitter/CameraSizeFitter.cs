using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using BallsToCup.Core;
using BallsToCup.Core.Gameplay;
using BallsToCup.General;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace MyNamespace
{
    public class CameraSizeFitter : MonoBehaviour, IEventListener
    {
        #region Fields

        private Camera _camera;
        private Transform _tubeTransform;
        private MeshRenderer _tubeRender;
        [SerializeField] private MeshRenderer _groundRenderer;
        [SerializeField] private RectTransform _bottomBarBorder;
        [SerializeField] private Canvas _mainCanvas;
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
            await Task.Yield();
            var boundsInfo = _tubeEventController.onTubeBoundsRequest.GetFirstResult();
            if (boundsInfo == default)
                return;
            _tubeMax = boundsInfo.max;
            _tubeMin = boundsInfo.min;
            SetCameraSizeBaseOnCupAndTube();
        }

        private async void SetCameraSizeBaseOnCupAndTube()
        {
            CheckYAxis();
            CheckXAxis();
            await Task.Yield();
            CheckForCameraReposition();
            _levelManagerEventController.onCameraReady.Trigger();
        }

        void CheckForCameraReposition()
        {
            var bottomBarTopY = GetBottomScreen().y + (GetTopScreen().y - GetBottomScreen().y) *
                _bottomBarBorder.sizeDelta.y / _mainCanvas.GetComponent<RectTransform>().sizeDelta.y;
            var deltaPos = _groundRenderer.bounds.min.y  - bottomBarTopY;
            _camera.transform.position += deltaPos * Vector3.up;
        }

        void CheckYAxis()
        {
            var cupMin = _groundRenderer.bounds.min;
            if (_tubeMax.y <= GetTopScreen().y)
                return;
            var length = (_tubeMax.y - cupMin.y) * 1.2f;
            _camera.orthographicSize *= length / (GetTopScreen().y - GetBottomScreen().y);
        }

        void CheckXAxis()
        {
            var screenLeft = GetBottomScreen().x;
            var screenRight = GetTopScreen().x;
            if (_tubeMax.x < screenRight && _tubeMin.x > screenLeft)
                return;
            var length = screenRight - screenLeft;
            if (_tubeMax.x > screenRight)
                length += _tubeMax.x - screenRight;
            if (_tubeMin.x < screenLeft)
                length += screenLeft - _tubeMin.x;
            length *= 1.2f;
            _camera.orthographicSize *= length / (screenRight - screenLeft);
        }

        Vector3 GetTopScreen()
        {
            return _camera.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, 0.0f));
        }

        Vector3 GetBottomScreen()
        {
            return _camera.ViewportToWorldPoint(Vector3.zero);
        }

        #endregion
    }
}