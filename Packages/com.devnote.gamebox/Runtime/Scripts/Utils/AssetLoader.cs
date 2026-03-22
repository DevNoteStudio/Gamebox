using Cysharp.Threading.Tasks;
using DevNote;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Gamebox
{
    public static class AssetLoader
    {

        public static async UniTask<Sprite> LoadItemSprite(ItemKey itemKey) 
            => await Addressables.LoadAssetAsync<Sprite>($"Items/{itemKey}");

        public static async UniTask<Sprite> LoadCardSprite(CardType cardType)
            => await Addressables.LoadAssetAsync<Sprite>($"Cards/{cardType}");

        public static async UniTask<Sprite> LoadLocationSprite(int locationIndex)
            => await Addressables.LoadAssetAsync<Sprite>($"Locations/{locationIndex}");


        public static async UniTask<Sprite> LoadUnlockSprite(UnlockKey unlockKey)
        {
            AssetReferenceT<Sprite> reference = null;

            if (unlockKey == UnlockKey.NoneStart) 
                reference = IConfigs.Gamebox.StartUnlockIconSpriteReference;

            else if (unlockKey == UnlockKey.NoneFinish)
                reference = IConfigs.Gamebox.FinishUnlockIconSpriteReference;

            else reference = IConfigs.Gamebox.GetUnlockIconSpriteReference(unlockKey);

            return await reference.LoadAssetWithKey();
        }





        public enum BoxSpriteType { Closed, Opened, FrontOpened }
        public static async UniTask<Sprite> LoadBoxSprite(ItemKey itemKey, BoxSpriteType spriteType)
        {
            switch (spriteType)
            {
                case BoxSpriteType.Closed:
                    return await LoadItemSprite(itemKey);

                case BoxSpriteType.Opened:
                    return await Addressables.LoadAssetAsync<Sprite>($"Boxes/{itemKey}_opened");

                case BoxSpriteType.FrontOpened:
                    return await Addressables.LoadAssetAsync<Sprite>($"Boxes/{itemKey}_front");
            }

            return null;
        }


    }
}
