using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace BallsToCup.General
{
    public interface IGeneralSettingsPersistentHandler
    {
         Task<GeneralSettingsModel> LoadSettingsData();
         Task<bool> SaveSettingsData(GeneralSettingsModel settingsData);
    }
}
