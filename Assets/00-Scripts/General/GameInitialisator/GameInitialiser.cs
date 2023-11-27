using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BallsToCup.General
{
    public class GameInitialiser
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void CheckForDebugActivation()
        {

#if UNITY_EDITOR
            Debug.unityLogger.logEnabled = true;
            return;
#endif
#if DEVELOPMENT_BUILD
              Debug.unityLogger.logEnabled = true;
            return;
#endif
            Debug.unityLogger.logEnabled = false;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void CheckForStartFromInitialScene()
        {
// #if UNITY_EDITOR
            var settings = Resources.Load<GameInitialisationSettings>("Settings/GameInitialisationSettings");
            if (!settings.forceStartFromMetaScene)
                return;
         
            SceneManager.LoadScene(0);
// #endif
        }
    }
}
