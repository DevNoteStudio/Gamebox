using DevNote;

namespace Gamebox
{
    public class CardsController
    {
        
        private readonly Viewer<CardsScreenView> cardsScreenViewer;



        public CardsController()
        {
            cardsScreenViewer = new(IConfigs.GetViewPrefab<CardsScreenView>());


        }

        public void ShowCardsScreen()
        {
            cardsScreenViewer.ShowExpand(UI.Container).Display();
        }

        public void HideCardsScreen() 
        {
            cardsScreenViewer.Hide();
        }




    }
}
