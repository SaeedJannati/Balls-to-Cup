using BallsToCup.General.Popups;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ProjectContextScriptableObjectInstaller", menuName = "Installers/ProjectContextScriptableObjectInstaller")]
public class ProjectContextScriptableObjectInstaller : ScriptableObjectInstaller<ProjectContextScriptableObjectInstaller>
{
    #region Fields

    [SerializeField, Expandable] private PopupManagerModel _PopupManagerModel;
    

    #endregion

    #region Methods

    public override void InstallBindings()
    {
        Container.Bind<PopupManagerModel>().FromScriptableObject(_PopupManagerModel).AsSingle();
    }

    #endregion
}