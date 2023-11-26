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
            Container.BindFactory<CoreBallView, CoreBallLogic, CoreBallLogic.Factory>();
            
        }

        private void BindEventControllers()
        {
            Container.BindInterfacesAndSelfTo<FlowControllerEventController>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelManagerEventController>().AsSingle();
            Container.BindInterfacesAndSelfTo<TubeEventController>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameManagerEventController>().AsSingle();
        }

        private void BindManagers()
        {
            Container.BindInterfacesAndSelfTo<FlowController>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<TubeGenerator>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
        }

        #endregion
    }
}