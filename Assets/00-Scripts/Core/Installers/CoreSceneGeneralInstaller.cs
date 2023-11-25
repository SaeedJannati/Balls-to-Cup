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
            BindFactories();
        }

        private void BindFactories()
        {
            Container.BindFactory<TubeView, TubeLogic, TubeLogic.Factory>();
        }

        private void BindEventControllers()
        {
            Container.BindInterfacesAndSelfTo<FlowControllerEventController>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelManagerEventController>().AsSingle();
            Container.BindInterfacesAndSelfTo<TubeEventController>().AsTransient();
        }

        private void BindManagers()
        {
            Container.BindInterfacesAndSelfTo<FlowController>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<TubeGenerator>().AsSingle();
            
        }

        #endregion
    }
}