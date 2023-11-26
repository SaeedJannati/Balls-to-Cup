using System;
using System.Collections;
using System.Collections.Generic;
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
            var ballPrefab = await LoadBallPrefab();
            SetPhysicsMaterialProperties(currentLevel);
            await Task.Yield();
            var pivotPos = _tubeEventController.onPivotTransformRequest.GetFirstResult();
            StartCoroutine(CreateBallsRoutine(currentLevel, pivotPos, ballPrefab));

        }

         IEnumerator CreateBallsRoutine(BallsToCupLevel currentLevel,Vector3 pivotPos,CoreBallView prefab)
         {
             var ballsCount = currentLevel.ballsCount;
             var counter = 0;
             var delay = new WaitForSeconds(.6f);
             var ballsDistance = currentLevel.ballDiameter * 1.03f;
             var deltaPos = Vector3.zero;
            while (true)
            {
                for (var i = -1; i < 2; i++)
                {
                    for (var j = -1; j < 2; j++)
                    {
                        for (var k = -1; k < 2; k++)
                        {
                            if (counter >= ballsCount)
                            {
                                _model.ball.ReleaseAsset();
                                _levelManagerEventController.onBallsGenerationComplete.Trigger();
                                yield break;
                            }

                            deltaPos.x = i * ballsDistance;
                            deltaPos.y = j * ballsDistance;
                            deltaPos.z = k * ballsDistance;
                            CreateBall(pivotPos + deltaPos, currentLevel, prefab);
                            counter++;
                        }
                    }
                }

                yield return delay;
            }
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