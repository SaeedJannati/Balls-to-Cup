using System;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

namespace  BallsToCup.General
{
    public class PrefHandler:IDisposable
    {
        #region Methods

        public bool SetPref<T>(string key,T value)
        {
            
            try
            {
                var serializedValue = JsonConvert.SerializeObject(value,settings:new()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
                PlayerPrefs.SetString(key,serializedValue);
                return true;
            }
            catch (Exception e)
            {
              BtcLogger.Log($"Couldn't set pref! key:{key}, exception:{e}","yellow");
              return false;
            }
        }

        public bool HasKey(string key) => PlayerPrefs.HasKey(key);

        public T GetPref<T>(string key, T defaultValue = default)
        {
            if (!HasKey(key))
                return defaultValue;
            try
            {
                var rawValue = PlayerPrefs.GetString(key);
                return JsonConvert.DeserializeObject<T>(rawValue);
            }
            catch (Exception e)
            {
                BtcLogger.Log($"Couldn't get pref! key:{key}, exception:{e}","yellow");
                return defaultValue;
            }
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

    
    }    
}

