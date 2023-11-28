using Zenject;

namespace BallsToCupGeneral.Audio
{
    public class AudioSystemInstaller : Installer<AudioSystemInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AudioHandler>().AsSingle();
            Container.Bind<AudioHandlerDataExtractor>().AsSingle();
            Container.Bind<AudioHandlerEventController>().AsSingle();
        }
    }
}
