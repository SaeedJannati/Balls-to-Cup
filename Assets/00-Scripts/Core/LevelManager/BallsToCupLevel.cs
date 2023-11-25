using System;
using System.Collections.Generic;
using BallsToCup.General.Editor;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace BallsToCup.Core.Gameplay
{
    public class BallsToCupLevel : ScriptableObject
    {
        #region Properties

        [SerializeField,InspectorReadOnly] public int index;
        [field: SerializeField] public int ballsCount { get; private set; }
        [field: SerializeField] public float controllerSensitivity { get; private set; } = 1.0f;
        [field: SerializeField] public float ballDiameter { get; private set; } = .15f;
        [field: SerializeField] public float ballsFriction { get; private set; } = .6f;
        [field: SerializeField] public float ballMass { get; private set; } = 1.0f;
        [field: SerializeField] public float gravity { get; private set; } =9.81f;
        [field: SerializeField] public float tubeDistanceToGround { get; private set; }
        [field: SerializeField] public List<StarRatio> starRatios { get; private set; } = new();
        [field: SerializeField] public AssetReference tube { get; private set; } = new();

        #endregion

        #region Methods

        public void OnValidate()
        {
#if UNITY_EDITOR
            for (int i = 0, e = starRatios.Count; i < e; i++)
            {
                starRatios[i].OnValidate(i,ballsCount);
            }

            EditorUtility.SetDirty(this);
#endif
        }

        public void SetIndex(int pIndex)
        {
            index = pIndex;
        }

        #endregion

        #region LocalClasses

        [Serializable]
        public class StarRatio
        {
            [SerializeField,InspectorReadOnly] private string _index;
            [SerializeField,InspectorReadOnly] private float _ballsRatioPercentage;
            [field: SerializeField] public int requiredBalls { get; private set; }

            public void OnValidate(int index, int totalBalls)
            {
#if UNITY_EDITOR
                _ballsRatioPercentage = (float)requiredBalls / totalBalls * 100.0f;
                _index = index.ToString();
#endif
            }
        }

        #endregion
    }
}