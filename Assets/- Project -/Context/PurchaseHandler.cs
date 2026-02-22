using System.Collections.Generic;
using DevNote;
using UnityEngine;
using Gamebox;

public class PurchaseHandler : MonoBehaviour, IPurchaseHandler
{
    List<ProductKey> IPurchaseHandler.ConsumableProductKeys => new();

    void IPurchaseHandler.HandlePurchase(ProductKey productKey)
    {
        switch (productKey)
        {
            case ProductKey.NoAds:
                DevNote.IGameState.NoAdsPurchased.Value = true;
                break;

            case ProductKey.Gems1: case ProductKey.Gems2: case ProductKey.Gems3:
            case ProductKey.Gems4: case ProductKey.Gems5: case ProductKey.Gems6:
                int gems = IConfigs.Gamebox.GetGemsInsidePack(productKey);
                Gamebox.IGameState.Items.Add(ItemKey.Gems, gems);
                break;

            default:
                Debug.LogWarning($"Handle for product {productKey} does not exist!");
                break;
        }
    }

    bool IPurchaseHandler.ProductIsPurchased(ProductKey productKey) => productKey switch
    {
        ProductKey.NoAds => DevNote.IGameState.NoAdsPurchased.Value,
        _ => false,
    };

}

