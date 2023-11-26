using Zenject;

namespace BallsToCup.General.Popups
{
    public class PopupManagerInstaller : Installer<PopupManagerInstaller>
    {
        #region Methods
        public override void InstallBindings()
        {
            InstallManagers();
        }

        void InstallManagers()
        {
            Container.Bind<PopupManager>().AsSingle();
        }

        #endregion
     
    }
}