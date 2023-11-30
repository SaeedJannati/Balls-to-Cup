using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace BallsToCup.Meta.UI
{
    public class MetaSelectLevelPanelModel : ScriptableObject
    {
        #region Properties

        [field: SerializeField] public float fadePeriod { get; private set; } = .5f;
        [field: SerializeField] public float levelItemScalePeriod { get; private set; } = .2f;
        [field: SerializeField] public float levelItemGenerationDelayInBetween { get; private set; } = .08f;
        [field: SerializeField] public AssetReference levelViewReference { get; private set; }

        #endregion
    }
}
