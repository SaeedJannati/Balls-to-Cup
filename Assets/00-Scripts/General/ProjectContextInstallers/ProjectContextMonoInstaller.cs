using BallsToCup.General.Popups;
using BallsToCupGeneral.Audio;
using UnityEngine;
using Zenject;

namespace BallsToCup.General
{
    public class ProjectContextMonoInstaller : MonoInstaller
    {
        #region Fields
        [SerializeField] private CoroutineHelper _coroutineHelper;
        

        #endregion
        #region Methods

        public override void InstallBindings()
        {
            InstallInstallers();
            BindMonoBehaviours();
        }

        private void BindMonoBehaviours()
        {
            Container.Bind<CoroutineHelper>().FromInstance(_coroutineHelper);
        }

        private void InstallInstallers()
        {
            ProjectContextGeneralInstaller.Install(Container);
            PopupsInstaller.Install(Container);
            PopupManagerInstaller.Install(Container);
            AudioSystemInstaller.Install(Container);
            GeneralSettingsInstaller.Install(Container);
        }

        #endregion
    }
}
