using System;
using System.Threading.Tasks;
using BallsToCup.General;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace BallsToCup.Core.Gameplay
{
    public class TubeGenerator : IDisposable, IInitializable, IEventListener
    {
        #region Fields

        [Inject] private LevelManagerEventController _levelManagerEventController;
        [Inject] private TubeLogic.Factory _tubeLogicFactory;
        #endregion

        #region Methods

        public void Dispose()
        {
            UnregisterFromEvents();
            GC.SuppressFinalize(this);
        }

        public void Initialize()
        {
            RegisterToEvents();
        }

        public void RegisterToEvents()
        {
            _levelManagerEventController.onCreateTubeRequest.Add(OnTubeCreateRequest);
        }

        public void UnregisterFromEvents()
        {
            _levelManagerEventController.onCreateTubeRequest.Remove(OnTubeCreateRequest);
        }

        private async void OnTubeCreateRequest()
        {
            var currentLevel = _levelManagerEventController.onCurrentLevelRequest.GetFirstResult();
            if(currentLevel==default)
                return;
            var oprtn =Addressables.LoadAssetAsync<GameObject>(currentLevel.tube);
            while (!oprtn.IsDone)
            {
                await Task.Yield();
            }

            if (oprtn.Result == default)
                throw new Exception("GameObject is null");
            if (!oprtn.Result.TryGetComponent(out TubeView tubeView))
            {
                currentLevel.tube.ReleaseAsset();
                return;
            }
         
            var view = GameObject.Instantiate(tubeView);
            view.tubePivot.position = new Vector3(0, currentLevel.tubeDistanceToGround,0 );
            _tubeLogicFactory.Create(view);
            currentLevel.tube.ReleaseAsset();
            _levelManagerEventController.onTubeCreated.Trigger();
        }

        #endregion
    }
}