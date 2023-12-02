using System;
using System.Runtime.InteropServices;
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
        [Inject] private AddressableLoader _addressableLoader;
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

            var view = await CreateTubeView(currentLevel);
          
           
            _tubeLogicFactory.Create(view);
            currentLevel.tube.ReleaseAsset();
            _levelManagerEventController.onTubeCreated.Trigger();
        }

        async Task<TubeView> CreateTubeView(BallsToCupLevel level)
        {
            var tubePrefab = await _addressableLoader.LoadAssetReference(level.tube);
            if (!tubePrefab.TryGetComponent(out TubeView tubeView))
            {
                level.tube.ReleaseAsset();
                throw new Exception($"Tube view component not found on the tube prefab of level:{level.index}!");
            }
            var view = GameObject.Instantiate(tubeView);
            view.tubePivot.position = new Vector3(0, level.tubeDistanceToGround,0 );
            return view;
        }

        #endregion
    }
}