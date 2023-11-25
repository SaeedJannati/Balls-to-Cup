using UnityEngine;
using Zenject;

namespace BallsToCup.Core.Installers
{
    public class CoreSceneMonoInstaller : MonoInstaller
    {
        #region Fields

        [SerializeField] private SwipeInput _swipeInput;
        

        #endregion

        #region Methods
        public override void InstallBindings()
        {
            BindInterfaces();
            BindInstallers();
        }

        private void BindInstallers()
        {
           CoreSceneGeneralInstaller.Install(Container);
        }

        private void BindInterfaces()
        {
            Container.Bind<IDraggable>().FromInstance(_swipeInput);
        }

        #endregion
     
    }
}
