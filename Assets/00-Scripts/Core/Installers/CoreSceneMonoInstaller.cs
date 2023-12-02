using BallsToCup.Core.Gameplay;
using BallsToCup.Meta.Levels;
using UnityEngine;
using Zenject;

namespace BallsToCup.Core.Installers
{
    public class CoreSceneMonoInstaller : MonoInstaller
    {
        #region Fields

        [SerializeField] private SwipeInput _swipeInput;
        [SerializeField] private BallGenerator _ballGenerator;
        [SerializeField] private YCriterion _yCriterion;
        [SerializeField] private LevelExtractor _levelExtractor;
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
            Container.Bind<IDraggable>().FromInstance(_swipeInput).AsSingle();
            Container.Bind<BallGenerator>().FromInstance(_ballGenerator).AsSingle();
            Container.Bind<YCriterion>().FromInstance(_yCriterion).AsSingle();
            Container.Bind<LevelExtractor>().FromInstance(_levelExtractor).AsSingle();
            
        }

        #endregion
     
    }
}
