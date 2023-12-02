using UnityEngine;
using UnityEngine.AddressableAssets;

namespace BallsToCup.Meta.Levels
{
    public class LevelExtractorModel:ScriptableObject
    {
        #region Properties

        [field:SerializeField] public GameObject pointOnPath { get; private set; }

        [field:SerializeField] public Material material{ get; private set; }
        [field:SerializeField] public AssetReference tubeCompositePrefab{ get; private set; }
        [field: SerializeField] public float tubeRadius { get; private set; } = 8.0f;
        [field: SerializeField] public int radialSegmentsCount { get; private set; } = 8;

        #endregion

    }
}