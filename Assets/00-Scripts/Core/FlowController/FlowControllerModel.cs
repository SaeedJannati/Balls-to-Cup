using UnityEngine;
using UnityEngine.AddressableAssets;

namespace BallsToCup.Core.Gameplay
{
    public class FlowControllerModel:ScriptableObject
    {
        #region Properties

        [field: SerializeField] public AssetReference winGameVfx { get; private set; }


        #endregion
    }
}