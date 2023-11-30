using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace BallsToCup.Core.Gameplay
{
    public class BallGeneratorModel:ScriptableObject
    {
        #region Properties

        [field: SerializeField] public AssetReference ball { get; private set; }

        [field: SerializeField] public PhysicMaterial ballsPhysiscMaterial { get; private set; }
        [field: SerializeField] public List<Material> ballMaterials { get; private set; }
        [field: SerializeField] public float maxTubeRotVelocity { get; private set; }
        #endregion
    }
}