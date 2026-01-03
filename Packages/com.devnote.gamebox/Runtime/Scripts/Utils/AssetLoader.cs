using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Gamebox
{
    public static class AssetLoader
    {
        

        public static async UniTask<Sprite> LoadItemSprite(ItemKey itemKey) 
            => await Addressables.LoadAssetAsync<Sprite>($"{itemKey}_sprite");



    }
}
