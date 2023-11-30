using System.Threading.Tasks;
using Zenject;

namespace BallsToCup.General
{
    public class PrefGeneralSettingsPersistentHandler : IGeneralSettingsPersistentHandler
    {
        #region Fields

        [Inject] private PrefHandler _prefHandler;

        #endregion

        #region Methods

        public async Task<GeneralSettingsModel> LoadSettingsData()
        {
            return _prefHandler.GetPref<GeneralSettingsModel>(PrefKeys.GeneralSettings.generalSettingsKey, default);
        }

        public async Task<bool> SaveSettingsData(GeneralSettingsModel settingsData)
        {
            return _prefHandler.SetPref(PrefKeys.GeneralSettings.generalSettingsKey, settingsData);
        }

        #endregion

        #region Factory

        public class Factory : PlaceholderFactory<PrefGeneralSettingsPersistentHandler>
        {
        }

        #endregion
    }
}