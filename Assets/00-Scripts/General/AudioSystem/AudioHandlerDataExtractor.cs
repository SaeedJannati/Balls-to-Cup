using System.Linq;
using UnityEngine.SceneManagement;
using Zenject;

namespace BallsToCupGeneral.Audio
{
    public class AudioHandlerDataExtractor
    {
        #region Fields

        [Inject] private AudioHandlerClipLibrary _model;

        #endregion

        #region Methods

        public AudioHandlerDataExtractor SetModel(AudioHandlerClipLibrary model)
        {
            _model = model;
            return this;
        }

        public SceneAudioLibrary GetSceneAudioLibrary(string sceneName)
        {
            var gameSceneCheck = CheckForGameScene(sceneName);
            return gameSceneCheck.library;
        }

        public SceneAudioLibrary LoadGeneralLibrary()
        {
            return GetGeneralAudioLibrary();
        }


        (bool isGameScene, SceneAudioLibrary library) CheckForGameScene(string sceneName)
        {
            var library = GetSceneAudioLibrary();
            if (library == default)
                return (false, new());
            return (true, library);
        }

        SceneAudioLibrary GetGeneralAudioLibrary()
        {
            var library = _model.audioLibraryInfos.FirstOrDefault(i => i.audioLibrary.isGeneralLibrary);
            return library != default ? library.audioLibrary : default;
        }


        SceneAudioLibrary GetSceneAudioLibrary()
        {
            var activeScene = SceneManager.GetActiveScene();
            var library = _model.audioLibraryInfos
                .Select(i => i.audioLibrary)
                .Where(i => !i.isGeneralLibrary)
                .FirstOrDefault(j => j.sceneName == activeScene.name);
            return library ? library : new();
        }
        #endregion
    }
}