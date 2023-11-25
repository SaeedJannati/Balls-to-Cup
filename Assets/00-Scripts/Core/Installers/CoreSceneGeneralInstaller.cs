using UnityEngine;
using Zenject;

namespace BallsToCup.Core.Installers
{
    public class CoreSceneGeneralInstaller : Installer<CoreSceneGeneralInstaller>
    {
        #region Methods

        public override void InstallBindings()
        {
            BindManagers();
            BindEventControllers();
        }

        private void BindEventControllers()
        {
            
            
        }

        private void BindManagers()
        {
      
        }

        #endregion
    }
}