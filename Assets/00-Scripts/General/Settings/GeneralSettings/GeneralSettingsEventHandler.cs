

namespace  BallsToCup.General
{
    public class GeneralSettingsEventHandler : BaseEventController
    {
        public ListFuncEvent<AudioSettings> onAudioSettingsRequest = new();
        public ListEvent<AudioSettings> onAudioSettingsSaveRequest = new();
    }
}

