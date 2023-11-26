
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BallsToCup.General
{
    public static  class ScriptableObjectCreator 
    {
#if UNITY_EDITOR
        [MenuItem("Balls to cup/Create ScriptableObject")]
        public static void Create()
        {

            var script = Selection.activeObject as MonoScript;
            if(script is null)
                return;
            var type = script.GetClass();
            var scriptableObject = UnityEngine. ScriptableObject.CreateInstance(type);
            var path = Path.GetDirectoryName(AssetDatabase.GetAssetPath(script));
            AssetDatabase.CreateAsset(scriptableObject, $"{path}/{Selection.activeObject.name}.asset");
        }
#endif
    }
}
