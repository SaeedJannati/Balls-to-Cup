using System;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

namespace BallsToCup.General
{
    public class GeneralObjectPool : IDisposable
    {
        #region Fields

        private readonly Dictionary<Object, List<GameObject>> _objectPool =
            new();

        #endregion

        #region Methods

        public GameObject GetObject(Object prefab)
        {
            if (_objectPool.ContainsKey(prefab))
            {
                for (int i = _objectPool[prefab].Count - 1; i >= 0; i--)
                {
                    if (_objectPool[prefab][i] != null)
                    {
                        if (!_objectPool[prefab][i].activeInHierarchy)
                        {
                            _objectPool[prefab][i].transform.SetParent(null);
                            _objectPool[prefab][i].transform.localPosition = Vector3.zero;
                            _objectPool[prefab][i].transform.localRotation = Quaternion.identity;
                            _objectPool[prefab][i].SetActive(true);
                            return _objectPool[prefab][i];
                        }
                    }
                    else
                    {
                        _objectPool[prefab].RemoveAt(i);
                    }
                }

                GameObject go = GameObject.Instantiate((GameObject)prefab);
                _objectPool[prefab].Add(go);
                return go;
            }
            else
            {
                List<GameObject> list = new List<GameObject>();
                GameObject go = GameObject.Instantiate((GameObject)prefab);
                list.Add(go);
                _objectPool.Add(prefab, list);
                return go;
            }
        }


        public void PreWarmPool(Object prefab, int instanceCount)
        {
            if (_objectPool.ContainsKey(prefab))
            {
                if (_objectPool[prefab].Capacity > instanceCount)
                    return;
                else
                {
                    _objectPool[prefab].Capacity = instanceCount;
                    for (int i = _objectPool[prefab].Count - 1; i >= 0; i++)
                    {
                        if (_objectPool[prefab][i] == null)
                        {
                            _objectPool[prefab].RemoveAt(i);
                        }
                    }

                    for (int i = _objectPool[prefab].Count - 1; i < instanceCount; i++)
                    {
                        GameObject go = GameObject.Instantiate((GameObject)prefab, null);
                        go.SetActive(false);
                        _objectPool[prefab].Add(go);
                    }
                }
            }
            else
            {
                List<GameObject> objs = new List<GameObject>(instanceCount);
                for (int i = 0; i < instanceCount; i++)
                {
                    GameObject go = GameObject.Instantiate((GameObject)prefab, null);
                    go.SetActive(false);
                    objs.Add(go);
                }

                _objectPool.Add(prefab, objs);
            }
        }

        public void ClearPool()
        {
            foreach (var pool in _objectPool.Values)
            {
                foreach (var obj in pool)
                {
                    GameObject.Destroy(obj);
                }
            }

            _objectPool.Clear();
        }

        public void Dispose()
        {
            ClearPool();
        }

        #endregion
    }
}