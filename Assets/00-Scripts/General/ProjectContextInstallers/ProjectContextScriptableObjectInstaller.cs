using BallsToCup.General;
using BallsToCup.General.Popups;
using BallsToCupGeneral.Audio;
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
    [SerializeField, Expandable] private GeneralSettingsModel _generalSettingsModel;
    [SerializeField, Expandable] private AudioHandlerClipLibrary _audioHandlerClipLibrary;
    [SerializeField, Expandable] private AudioHandlerModel _audioHandlerModel; 
    #endregion

    #region Methods

    public override void InstallBindings()
    {
        Container.Bind<PopupManagerModel>().FromScriptableObject(_PopupManagerModel).AsSingle();
        Container.Bind<GameResultPanelModel>().FromScriptableObject(_gameResultPanelModel).AsSingle();
        Container.Bind<PlayerProgressManagerModel>().FromScriptableObject(_playerProgressModel).AsSingle();
        Container.Bind<GeneralSettingsModel>().FromScriptableObject(_generalSettingsModel).AsSingle();
        Container.Bind<AudioHandlerClipLibrary>().FromScriptableObject(_audioHandlerClipLibrary).AsSingle();
        Container.Bind<AudioHandlerModel>().FromScriptableObject(_audioHandlerModel).AsSingle();
        
    }

    #endregion
}