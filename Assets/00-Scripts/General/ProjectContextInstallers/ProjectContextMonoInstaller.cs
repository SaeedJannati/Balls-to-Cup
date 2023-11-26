using BallsToCup.General.Popups;
using UnityEngine;
using Zenject;

namespace BallsToCup.General
{
    public class ProjectContextMonoInstaller : MonoInstaller
    {
        #region Methods

        public override void InstallBindings()
        {
            InstallInstallers();
   
        }

        private void InstallInstallers()
        {
            ProjectContextGeneralInstaller.Install(Container);
            PopupsInstaller.Install(Container);
            PopupManagerInstaller.Install(Container);
        }

        #endregion
    }
}
