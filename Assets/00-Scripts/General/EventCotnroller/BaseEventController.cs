using System;
using System.Collections.Generic;
using UnityEngine;

namespace BallsToCup.General
{
    public abstract partial class BaseEventController:IDisposable
    {
     
        #region ListVersion

        #region EventCalsses

        public class ListFuncEvent<T>
        {
            #region Fields

            private readonly List<Func<T>> _listeners = new();

            #endregion

            #region Methods

            public void Add(Func<T> listener)
            {
                _listeners.Add(listener);
            }

            public void Remove(Func<T> listener)
            {
                _listeners.Remove(listener);
            }
            [HideInCallstack]
            public List<T> Trigger()
            {
                if (_listeners.Count == 0)
                    return default;
                var outPut = new List<T>(_listeners.Count);
                for (int i = 0, e = _listeners.Count; i < e; i++)
                {
                    if (_listeners[i] == default)
                        continue;
                    outPut.Add(_listeners[i].Invoke());
                }

                return outPut;
            }

            [HideInCallstack]
            public T GetFirstResult()
            {
                var results = Trigger();
                if (results == default)
                    return default;
                if (results.Count == 0)
                    return default;
                return results[0];
            }
            
            #endregion
        }

        public class ListFuncEvent<TIn, TOut>
        {
            #region Fields

            private readonly List<Func<TIn, TOut>> _listeners = new();

            #endregion

            #region Methods

            public void Add(Func<TIn, TOut> listener)
            {
                _listeners.Add(listener);
            }

            public void Remove(Func<TIn, TOut> listener)
            {
                _listeners.Remove(listener);
            }
            [HideInCallstack]
            public List<TOut> Trigger(TIn input)
            {
                if (_listeners.Count == 0)
                    return default;
                var outPut = new List<TOut>(_listeners.Count);
                for (int i = 0, e = _listeners.Count; i < e; i++)
                {
                    if (_listeners[i] == default)
                        continue;
                    outPut.Add(_listeners[i].Invoke(input));
                }

                return outPut;
            }
            [HideInCallstack]
           public TOut GetFirstResult(TIn input)
            {
                var results = Trigger(input);
                if (results == default)
                    return default;
                if (results.Count == default)
                    return default;
                return results[0];
            }

            #endregion
        }

        public class ListEvent
        {
            #region Fields

            private readonly List<Action> _listeners = new();

            #endregion

            #region Methods

            public void Add(Action listener)
            {
                _listeners.Add(listener);
            }

            public void Remove(Action listener)
            {
                _listeners.Remove(listener);
            }
            [HideInCallstack]
            public void Trigger()
            {
                if (_listeners.Count == 0)
                    return;

                for (int i = 0, e = _listeners.Count; i < e; i++)
                {
                    _listeners[i]?.Invoke();
                }
            }

            #endregion
        }
        public class ListEvent<T>
        {
            #region Fields

            private readonly List<Action<T>> _listeners = new();

            #endregion

            #region Methods

            public void Add(Action<T> listener)
            {
                _listeners.Add(listener);
            }

            public void Remove(Action<T> listener)
            {
                _listeners.Remove(listener);
            }
            [HideInCallstack]
            public void Trigger(T input)
            {
                if (_listeners.Count == 0)
                    return;

                for (int i = 0, e = _listeners.Count; i < e; i++)
                {
                    _listeners[i]?.Invoke(input);
                }
            }

            #endregion
        }
        #endregion

        #endregion

        #region Methods

        public  void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        protected virtual void ReleaseUnmanagedResources()
        {
            
        }

        #endregion
    }
}