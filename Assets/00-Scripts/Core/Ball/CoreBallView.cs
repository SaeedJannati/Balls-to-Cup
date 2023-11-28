using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallsToCup.Core.Gameplay
{
    public class CoreBallView : MonoBehaviour
    {
        #region Properties

        [field: SerializeField] public Transform ballTransform { get; private set; }
        [field: SerializeField] public Rigidbody ballRigidBody { get; private set; }
        [field: SerializeField] public MeshRenderer ballRenderer { get; private set; }
        private Action _onGoBelowYCriterion;
        private float _yCriterion;
        #endregion

        #region unity actions

        private void Start()
        {
            StartCoroutine(CheckYCriterionRoutine());
        }

        #endregion
        #region Methods

        public CoreBallView SetYCriterion(float criterion)
        {
            _yCriterion = criterion;
            return this;
        }

        public CoreBallView SetGoBelowYCriterionAction(Action onGoBelowYCriterion)
        {
            _onGoBelowYCriterion = onGoBelowYCriterion;
            return this;
        }

        IEnumerator CheckYCriterionRoutine()
        {
            var isBelowCriterion = false;
            var delay = new WaitForSeconds(1.0f);
            while (!isBelowCriterion)
            {
                isBelowCriterion=ballTransform.position.y < _yCriterion;
                yield return delay;
            }
            _onGoBelowYCriterion?.Invoke();
        }

        #endregion
      
    }
}