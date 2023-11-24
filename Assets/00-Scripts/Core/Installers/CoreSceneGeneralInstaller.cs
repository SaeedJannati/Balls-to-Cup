using UnityEngine;
using Zenject;

namespace BallsToCup.Core
{
    public class CoreSceneGeneralInstaller : MonoInstaller
    {
        #region Fields

        [SerializeField] private SwipeInput _swipeInput;
        

        #endregion

        #region Methods
        public override void InstallBindings()
        {
            BindInterfaces();
        }

        private void BindInterfaces()
        {
            Container.Bind<IDraggable>().FromInstance(_swipeInput);
        }

        #endregion
     
    }
}
