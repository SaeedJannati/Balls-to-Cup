using Zenject;

namespace BallsToCup.General
{
    public class GeneralSettingsInstaller : Installer<GeneralSettingsInstaller>
    {
        public override void InstallBindings()
        {
            BindLogics();
            BindEventControllers();
            BindFactories();
            BindInterfaces();
        }

        private void BindLogics()
        {
            Container.BindInterfacesAndSelfTo<GeneralSettingsHandler>().AsSingle();
        
        }


  
        private void BindEventControllers()
        {
            Container.Bind<GeneralSettingsEventHandler>().AsSingle();
        }

        private void BindFactories()
        {
            Container.BindFactory<PrefGeneralSettingsPersistentHandler, PrefGeneralSettingsPersistentHandler.Factory>();
        }

        private void BindInterfaces()
        {
            Container.Bind<IGeneralSettingsPersistentHandler>().FromFactory<GeneralPersistantHandlerFactory>().AsSingle();
        }
    }

    internal class GeneralPersistantHandlerFactory:IFactory<IGeneralSettingsPersistentHandler>
    {
        #region Fields

        [Inject] private PrefGeneralSettingsPersistentHandler.Factory _prefGeneralSettingsPersistantFactory;

        #endregion
        public IGeneralSettingsPersistentHandler Create()
        {
            return _prefGeneralSettingsPersistantFactory.Create();
        }
    }
}
