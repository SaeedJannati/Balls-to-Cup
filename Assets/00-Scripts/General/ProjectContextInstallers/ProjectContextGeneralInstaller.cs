using UnityEngine;
using Zenject;

namespace BallsToCup.General
{
    public class ProjectContextGeneralInstaller : Installer<ProjectContextGeneralInstaller>
    {
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
            Container.BindInterfacesAndSelfTo<PrefHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<AddressableLoader>().AsSingle();
            Container.BindInterfacesAndSelfTo<SceneLoader>().AsSingle();
            
        }
    }
}