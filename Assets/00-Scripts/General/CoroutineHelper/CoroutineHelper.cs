using System;
using System.Collections;
using UnityEngine;

namespace BallsToCup.General
{
    public class CoroutineHelper : MonoBehaviour
    {

        #region Methods

        public Coroutine FireCoroutine(IEnumerator coroutine) => StartCoroutine(coroutine);

        public void DelayedCallBack(Action action, float delay) =>
            StartCoroutine(DelayedCallBackCoroutine(action, delay));

        IEnumerator DelayedCallBackCoroutine(Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }

        #endregion

    }
}

