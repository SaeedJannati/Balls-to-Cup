using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BtcLogger.General
{
    public class ObjectPool<T> : IDisposable where T : MonoBehaviour
    {
        #region Fields

        protected List<T> _pool = new();
        protected Transform _poolParent;

        #endregion

        #region Methods

        public void SetParent(Transform parent)
        {
            _poolParent = parent;
        }

        public virtual void Prewarm(int poolCap)
        {
            ClearPool();
            for (int i = 0; i < poolCap; i++)
            {
                AddObjectToThePool();
            }
        }

        public virtual T GetObject()
        {
            var outPut = _pool.FirstOrDefault(i => !i.gameObject.activeInHierarchy);
            if (outPut != default)
            {
                outPut.gameObject.SetActive(true);
                return outPut;
            }

            AddObjectToThePool();
            _pool[^1].gameObject.SetActive(true);
            return _pool[^1];
        }

        protected virtual void AddObjectToThePool()
        {
            CheckForParent();
            var instance = new GameObject().AddComponent<T>();
            var index = _pool.Count;
            instance.name = $"{typeof(T)}-poolObject ({index})";
            instance.transform.SetParent(_poolParent);
            instance.gameObject.SetActive(false);
            _pool.Add(instance);
        }

        protected void CheckForParent()
        {
            if (_poolParent != default)
                return;
            var parent = new GameObject();
            parent.name = $"pool {typeof(T)}";
            _poolParent = parent.transform;
        }

        public virtual void ClearPool(bool destroy = true)
        {
            if (_pool.Count == 0)
                return;
            for (int i = _pool.Count-1 ;i >= 0; i--)
            {
                if (destroy)
                {
                    try
                    {
                        GameObject.Destroy(_pool[i].gameObject);
                    }
                    catch (Exception e)
                    {
                        
                    }
              
                    _pool.RemoveAt(i);
                    continue;
                }
                _pool[i].gameObject.SetActive(false);
                _pool.RemoveAt(i);
            }

            _pool = new();
        }

        public virtual void Dispose()
        {
            ClearPool();
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}