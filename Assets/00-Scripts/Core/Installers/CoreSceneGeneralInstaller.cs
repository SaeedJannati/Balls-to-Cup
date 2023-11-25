using BallsToCup.Core.Gameplay;
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
            Container.Bind<FlowControllerEventController>().AsSingle();
            Container.Bind<LevelManagerEventController>().AsSingle();
        }

        private void BindManagers()
        {
            Container.BindInterfacesAndSelfTo<FlowController>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelManager>().AsSingle();
        }

        #endregion
    }
}