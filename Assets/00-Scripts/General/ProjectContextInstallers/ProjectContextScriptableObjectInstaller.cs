using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ProjectContextScriptableObjectInstaller", menuName = "Installers/ProjectContextScriptableObjectInstaller")]
public class ProjectContextScriptableObjectInstaller : ScriptableObjectInstaller<ProjectContextScriptableObjectInstaller>
{
    public override void InstallBindings()
    {
    }
}