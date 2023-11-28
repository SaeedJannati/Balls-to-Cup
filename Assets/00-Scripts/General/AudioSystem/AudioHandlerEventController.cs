using BallsToCup.General;

namespace BallsToCupGeneral.Audio
{
    public class AudioHandlerEventController:BaseEventController
    {
        
        public readonly ListEvent<bool> onSfxTrigger=new();
        public readonly ListEvent<bool> onMusicTrigger=new();
    }
}