using System;
using Zenject;

namespace BallsToCup.General
{
    public class GeneralSettingsHandler:IDisposable
    {
        #region Fields

        [Inject] private GeneralSettingsModel _defaultGeneralSettings;
        [Inject] private GeneralSettingsEventHandler _eventHandler;
        [Inject] private IGeneralSettingsPersistentHandler _persistantDataHandler;
        private GeneralSettingsModel _generalSettings;

        #endregion

        #region Methods

        [Inject]
        void Initialise()
        {
            InitialiseSettingsModel();
            RegisterToEvents();
        }

       async void InitialiseSettingsModel()
       {
           _generalSettings = await _persistantDataHandler.LoadSettingsData();
            CheckForDefaultSettings();
        }

        private void CheckForDefaultSettings()
        {
            if(_generalSettings!=default)
                return;
            _generalSettings = _defaultGeneralSettings;
            _persistantDataHandler.SaveSettingsData(_generalSettings);
        }

        void RegisterToEvents()
        {
            _eventHandler.onAudioSettingsRequest.Add(OnAudioSettingsRequest);
            _eventHandler.onAudioSettingsSaveRequest.Add(OnAudioSettingsSaveRequest);
            _eventHandler.onDispose.Add(OnViewDestroy);
        }

        void UnregisterFromEvents()
        {
            _eventHandler.onAudioSettingsRequest.Remove(OnAudioSettingsRequest);
            _eventHandler.onAudioSettingsSaveRequest.Remove(OnAudioSettingsSaveRequest);
            _eventHandler.onDispose.Remove(OnViewDestroy);
        }

        private AudioSettings OnAudioSettingsRequest()
        {
            return _generalSettings.audioSettings;
        }

        private void OnAudioSettingsSaveRequest(AudioSettings audioSettings)
        {
            _generalSettings.audioSettings = audioSettings;
            _persistantDataHandler.SaveSettingsData(_generalSettings);
        }

        private void OnViewDestroy()
        {
            UnregisterFromEvents();
            Dispose();
        }
        public void Dispose()
        {
            _eventHandler?.Dispose();
            GC.SuppressFinalize(this);
        }
        #endregion

        
    }
}
namespace BallsToCup.General
{
    public partial class PrefKeys
    {
        public class GeneralSettings
        {
            public const string generalSettingsKey = "General_settings";
        }
    }
}
