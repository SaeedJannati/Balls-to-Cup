using BallsToCup.Core.Gameplay;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace BallsToCup.Core.Installers
{
    public class CoreScirptableObjectInstaller : ScriptableObjectInstaller<CoreScirptableObjectInstaller>
    {
        #region Fields

        [SerializeField, Expandable] private TubeRotatorModel _tubeRotatorModel;
        [SerializeField, Expandable] private LevelManagerModel _levelManagerModel;
        [SerializeField, Expandable] private BallGeneratorModel _ballGeneratorModel;
        #endregion

        #region Methods

        public override void InstallBindings()
        {
            Container.Bind<TubeRotatorModel>().FromScriptableObject(_tubeRotatorModel).AsSingle();
            Container.Bind<LevelManagerModel>().FromScriptableObject(_levelManagerModel).AsSingle();
            Container.Bind<BallGeneratorModel>().FromScriptableObject(_ballGeneratorModel).AsSingle();
        }

        #endregion
   
    }
}