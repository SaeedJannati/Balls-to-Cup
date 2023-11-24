using UnityEngine;
using Zenject;

namespace BtcLogger.General
{
    public class ObjectPoolFactory<T> : ObjectPool<T> where T : MonoBehaviour
    {

        #region Fields

        private PlaceholderFactory<T> _factory;


        #endregion

        #region Constructors

    

        public ObjectPoolFactory(PlaceholderFactory<T> factory)
        {
            _factory = factory;
        }

        #endregion

        #region Methods
        protected override void AddObjectToThePool()
        { 
            CheckForParent();
            var instance = _factory.Create();
            var index = _pool.Count;
            instance.name = $"{typeof(T)}-poolObject ({index})";
            instance.gameObject.SetActive(false);
            instance.transform.SetParent(_poolParent);
            _pool.Add(instance);
        }
        #endregion

        #region Factory

        public class Factory:PlaceholderFactory<PlaceholderFactory<T>,ObjectPoolFactory<T>>
        {
            
        }

        #endregion
    }
    
    
    

}