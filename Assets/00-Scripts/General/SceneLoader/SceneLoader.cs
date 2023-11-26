using System;
using UnityEngine.SceneManagement;
using Zenject;

namespace  BallsToCup.General
{
    public class SceneLoader:IDisposable
    {
        #region Methods


        public  void LoadScene(int sceneIndex,Action onComplete=default)
        {
            SceneManager.LoadSceneAsync(sceneIndex).completed += _ =>
            {
                onComplete?.Invoke();
            };
      
        }
  
        public void Dispose()
        {
          GC.SuppressFinalize(this);
        }
        #endregion

      
    }

  
}

