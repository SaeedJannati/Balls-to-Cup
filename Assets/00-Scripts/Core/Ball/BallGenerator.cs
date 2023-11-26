using System;
using System.Threading.Tasks;
using BallsToCup.General;
using NaughtyAttributes;
using UnityEngine;
using Zenject;
using Random = System.Random;

namespace BallsToCup.Core.Gameplay
{
    public class BallGenerator : MonoBehaviour, IEventListener
    {
        #region Fields

        [SerializeField, Expandable] private BallGeneratorModel _model;
        [Inject] private LevelManagerEventController _levelManagerEventController;
        [Inject] private AddressableLoader _addressableLoader;
        [Inject] private TubeEventController _tubeEventController;
        [Inject] private CoreBallLogic.Factory _ballLogicFactory;

        #endregion

        #region Unity actions

        private void OnDestroy()
        {
            UnregisterFromEvents();
        }

        #endregion

        #region MyRegion

        [Inject]
        public void Initialise()
        {
            RegisterToEvents();
        }

        public void RegisterToEvents()
        {
            _levelManagerEventController.onGenerateBallsRequest.Add(OnGenerateBallsRequest);
        }

        public void UnregisterFromEvents()
        {
            _levelManagerEventController.onGenerateBallsRequest.Remove(OnGenerateBallsRequest);
        }

        private async void OnGenerateBallsRequest()
        {
            gameObject.ClearChildren();
            var currentLevel = _levelManagerEventController.onCurrentLevelRequest.GetFirstResult();
            if (currentLevel == default)
                return;
            var pivotPos = _tubeEventController.onPivotTransformRequest.GetFirstResult();
            var ballPrefab = await LoadBallPrefab();
            SetPhysicsMaterialProperties(currentLevel);
            await Task.Yield();
            for (int i = 0, e = currentLevel.ballsCount; i < e; i++)
            {
                CreateBall(pivotPos, currentLevel, ballPrefab);
            }

            _model.ball.ReleaseAsset();
        }

        private void CreateBall(Vector3 pos, BallsToCupLevel currentLevel, CoreBallView prefab)
        {
            var view = Instantiate(prefab, transform);
            view.ballTransform.localScale = currentLevel.ballDiameter * Vector3.one;
            view.ballTransform.position = pos;
            view.ballRigidBody.mass = currentLevel.ballMass;
            view.ballRenderer.material = _model.ballMaterials[UnityEngine.Random.Range(0, _model.ballMaterials.Count)];
            _ballLogicFactory.Create(view);
        }

        private void SetPhysicsMaterialProperties(BallsToCupLevel currentLevel)
        {
            _model.ballsPhysiscMaterial.dynamicFriction = currentLevel.ballsFriction;
            _model.ballsPhysiscMaterial.staticFriction = currentLevel.ballsFriction;
            _model.ballsPhysiscMaterial.bounciness = currentLevel.ballsBounce;
        }


        private async Task<CoreBallView> LoadBallPrefab()
        {
            var ball = await _addressableLoader.LoadAssetReference(_model.ball);
            if (!ball.TryGetComponent(out CoreBallView view))
                throw new Exception("No ball view attached to ball prefab!");
            return view;
        }

        #endregion
    }
}