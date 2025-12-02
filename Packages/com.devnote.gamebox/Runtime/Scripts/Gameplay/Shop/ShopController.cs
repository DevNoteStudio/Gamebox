using DevNote;
using UnityEngine;

namespace Gamebox
{
    public class ShopController : MonoBehaviour
    {
        private readonly Viewer<ShopScreenView> shopScreenViewer;



        public ShopController()
        {
            shopScreenViewer = new(IConfigs.GetViewPrefab<ShopScreenView>());
        }


        public void ShowShopScreen()
        {
            shopScreenViewer.ShowExpand(UI.Container);
        }

        public void HideShopScreen()
        {
            shopScreenViewer.Hide();
        }



    }
}
