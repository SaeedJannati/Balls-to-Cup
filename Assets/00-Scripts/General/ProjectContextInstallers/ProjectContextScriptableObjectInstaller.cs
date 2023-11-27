using BallsToCup.General;
using BallsToCup.General.Popups;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ProjectContextScriptableObjectInstaller", menuName = "Installers/ProjectContextScriptableObjectInstaller")]
public class ProjectContextScriptableObjectInstaller : ScriptableObjectInstaller<ProjectContextScriptableObjectInstaller>
{
    #region Fields

    [SerializeField, Expandable] private PopupManagerModel _PopupManagerModel;
    [SerializeField, Expandable] private GameResultPanelModel _gameResultPanelModel;
    [SerializeField, Expandable] private PlayerProgressManagerModel _playerProgressModel;
    #endregion

    #region Methods

    public override void InstallBindings()
    {
        Container.Bind<PopupManagerModel>().FromScriptableObject(_PopupManagerModel).AsSingle();
        Container.Bind<GameResultPanelModel>().FromScriptableObject(_gameResultPanelModel).AsSingle();
        Container.Bind<PlayerProgressManagerModel>().FromScriptableObject(_playerProgressModel).AsSingle();
    }

    #endregion
}