using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace BtcLogger.General
{
    public class ObjectPoolAddressable<T> : ObjectPool<T> where T : MonoBehaviour
    {
        #region Fields

        private readonly AssetReference _assetReference;
        private T _prefab;

        #endregion

        #region Properties

        public bool initialised { get; private set; } = false;

        #endregion

        #region Constructor

        public ObjectPoolAddressable(AssetReference assetReference)
        {
            _assetReference = assetReference;
            LoadReference();
        }

        #endregion

        #region Methods

        public override async void Prewarm(int poolCap)
        {
            while (!initialised)
            {
                await Task.Yield();
            }

            base.Prewarm(poolCap);
        }

        private async void LoadReference()
        {
            var req = Addressables.LoadAssetAsync<GameObject>(_assetReference);
            while (!req.IsDone)
            {
                await Task.Yield();
            }
            if (req.Result == default)
                return;
            if (!req.Result.TryGetComponent(out _prefab))
                return;
            initialised = true;
        }

        protected override void AddObjectToThePool()
        {
            CheckForParent();
            if (!initialised)
                return;
            var instance = GameObject.Instantiate(_prefab,_poolParent);
            var index = _pool.Count;
            instance.name = $"{typeof(T)}-poolObject ({index})";
            instance.gameObject.SetActive(false);
            _pool.Add(instance);
        }

        public override T GetObject()
        {
            if (!initialised)
            {
                return default;
            }

            return base.GetObject();
        }

        public override void Dispose()
        {
            _assetReference.ReleaseAsset();
            base.Dispose();
        }

        #endregion
    }
}