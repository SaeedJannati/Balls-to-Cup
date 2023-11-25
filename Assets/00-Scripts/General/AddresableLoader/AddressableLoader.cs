using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace BallsToCup.General
{
    public class AddressableLoader:IDisposable
    {
        public async Task<GameObject> LoadAssetReference(AssetReference reference)
        {
            var oprtn =Addressables.LoadAssetAsync<GameObject>(reference);
            while (!oprtn.IsDone)
            {
                await Task.Yield();
            }

            if (oprtn.Result == default)
                throw new Exception("GameObject is null");
            var output = oprtn.Result;
            return output;
        }

        public async Task<Sprite> LoadSprite(AssetReferenceSprite reference)
        {
            var oprtn = Addressables.LoadAssetAsync<Sprite>(reference);
            while (!oprtn.IsDone)
            {
                await Task.Yield();
            }

            if (oprtn.Result == default)
                throw new Exception("Sprite is null");
            var output = oprtn.Result;
            return output;
        }

        public void Dispose()
        {
          GC.SuppressFinalize(this);
        }
    }
}