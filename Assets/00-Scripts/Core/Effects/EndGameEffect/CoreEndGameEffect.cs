using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallsToCup.Core.UI
{
    public class CoreEndGameEffect : MonoBehaviour
    {
        #region Fields

        [SerializeField] private GameObject _showerEffect;
        [SerializeField] private GameObject _blastEffect;
        [SerializeField] private List<GameObject> _directionalEffects;

        #endregion

        #region Methods

        public void ShowEffect()
        {
            StartCoroutine(ShowEffectRoutine());
        }

        private IEnumerator ShowEffectRoutine()
        {
            _directionalEffects.ForEach(i=>i.SetActive(true));
            yield return new WaitForSeconds(1f);
            _blastEffect.SetActive(true);
            yield return new WaitForSeconds(1.0f);
            _showerEffect.SetActive(true);
        }

        #endregion
    }
}

