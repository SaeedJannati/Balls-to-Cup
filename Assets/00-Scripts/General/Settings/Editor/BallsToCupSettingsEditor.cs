using BallsToCup.Core.Gameplay;
using BallsToCupGeneral.Audio;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace BallsToCup.General.Editor
{
    public class BallsToCupSettingsEditor : OdinMenuEditorWindow
    {
        #region Fields
        private static OdinMenuTree _menuTree;
        
        private const string levelEditorPath =
            "Assets/04-Models/Core/LevelManger/LevelManagerModel.asset";
        private const string audiohandlerModelPath =
            "Assets/04-Models/General/AudioSystem/AudioHandlerModel.asset";
        #endregion

        #region Methods
        [MenuItem("Balls to cup/Settings",false,0)]
        static void ShowSettingsWindow()
        {
            var window = GetWindow<BallsToCupSettingsEditor>("Balls to cup Settings");
            window.minSize = new Vector2(400, 400);
            window.Show();
        }
        protected override OdinMenuTree BuildMenuTree()
        {
            _menuTree = new OdinMenuTree();
            var buildSettings = AssetDatabase.LoadAssetAtPath(levelEditorPath, typeof(LevelManagerModel));
            _menuTree.Add("Level editor", buildSettings);
            var audioHandlerModel = AssetDatabase.LoadAssetAtPath(audiohandlerModelPath, typeof(AudioHandlerModel));
            _menuTree.Add("Audio Handler", audioHandlerModel);
            return _menuTree;
        }

        #endregion
  
    }
}

