using UnityEngine;

namespace BtcLogger.General
{
    public class ObjectPoolPrefab<T> : ObjectPool<T> where T : MonoBehaviour
    {
        #region Fields
        private readonly T _prefab;
        #endregion
        #region Constructor

        public ObjectPoolPrefab(T prefab)
        {
            _prefab = prefab;
        }

        #endregion

        #region Methods
        protected override void AddObjectToThePool()
        { 
            CheckForParent();
            var instance = GameObject.Instantiate(_prefab,_poolParent);
            var index = _pool.Count;
            instance.name = $"{typeof(T)}-poolObject ({index})";
            instance.gameObject.SetActive(false);
            _pool.Add(instance);
        }
        #endregion
    }
}