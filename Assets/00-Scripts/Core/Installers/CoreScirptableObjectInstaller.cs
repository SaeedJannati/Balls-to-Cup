using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace BallsToCup.Core
{
    public class CoreScirptableObjectInstaller : ScriptableObjectInstaller<CoreScirptableObjectInstaller>
    {
        #region Fields

        [SerializeField, Expandable] private TubeRotatorModel _tubeRotatorModel;
        

        #endregion

        #region Methods

        public override void InstallBindings()
        {
            Container.Bind<TubeRotatorModel>().FromScriptableObject(_tubeRotatorModel).AsSingle();
        }

        #endregion
   
    }
}