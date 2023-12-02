using System.IO;
using BallsToCup.General;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace BallsToCup.Meta.Levels
{
    public class LevelExtractorModel : ScriptableObject
    {
        #region Properties

        [field: SerializeField] public GameObject pointOnPath { get; private set; }

        [field: SerializeField] public Material material { get; private set; }
        [field: SerializeField] public AssetReference tubeCompositePrefab { get; private set; }
        [field: SerializeField] public float tubeRadius { get; private set; } = 8.0f;
        [field: SerializeField] public int radialSegmentsCount { get; private set; } = 8;
        [field: SerializeField] public int bezierSegmentPoinstCount { get; private set; } = 10;
        [field: SerializeField] public string svgFilesPath { get; private set; }

        #endregion

        #region Methods

        [Button]
        void ConvertSvgsToTxts()
        {
#if UNITY_EDITOR
            var path = Path.Combine(Application.dataPath, svgFilesPath);
            BtcLogger.Log(path);
            var info = new DirectoryInfo(path);
            var filesInfo = info.GetFiles("*.svg");
            foreach (var file in filesInfo)
            {
                BtcLogger.Log(file.FullName);
                File.Move(file.FullName, Path.ChangeExtension(file.FullName, ".txt"));
            }

            if (filesInfo.Length > 0)
                AssetDatabase.Refresh();
#endif
        }

        #endregion
    }
}