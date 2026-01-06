using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Gamebox
{
    public static class AssetLoader
    {
        

        public static async UniTask<Sprite> LoadItemSprite(ItemKey itemKey) 
            => await Addressables.LoadAssetAsync<Sprite>($"{itemKey}_sprite");


        public enum BoxSpriteType { Closed, Opened, FrontOpened }
        public static async UniTask<Sprite> LoadBoxSprite(ItemKey itemKey, BoxSpriteType spriteType)
        {
            switch (spriteType)
            {
                case BoxSpriteType.Closed:
                    return await LoadItemSprite(itemKey);

                case BoxSpriteType.Opened:
                    return await Addressables.LoadAssetAsync<Sprite>($"{itemKey}_opened");

                case BoxSpriteType.FrontOpened:
                    return await Addressables.LoadAssetAsync<Sprite>($"{itemKey}_front");
            }

            return null;
        }




    }
}
