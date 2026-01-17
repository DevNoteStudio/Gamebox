using UnityEngine;
using DevNote;
using System;

namespace Gamebox
{

    public class BoxAndCardTutorialController
    {
        private readonly MenuController menuController;
        private readonly ShopController shopController;
        private readonly BoxOpenController boxOpenController;
        private readonly CardsController cardsController;
        private readonly LevelController levelController;

        public BoxAndCardTutorialController(MenuController menuController, ShopController shopController,
            BoxOpenController boxOpenController, CardsController cardsController, LevelController levelController)
        {
            this.menuController = menuController;
            this.shopController = shopController;
            this.boxOpenController = boxOpenController;
            this.cardsController = cardsController;
            this.levelController = levelController;
        }


        public void StartTutorial()
        {
            if (IGameState.Items.Get(ItemKey.CommonBox) == 0)
                IGameState.Items.Add(ItemKey.CommonBox, 1);

            var shopTabButton = menuController.Tabs.GetTabButton(TabType.Shop);
            TutorialPointer.ShowPointer(shopTabButton.transform as RectTransform);

            shopTabButton.Button.onClick.AddListener(OnShopScreenOpened);
        }

        private void OnShopScreenOpened()
        {
            var shopTabButton = menuController.Tabs.GetTabButton(TabType.Shop);
            shopTabButton.Button.onClick.RemoveListener(OnShopScreenOpened);

            var tutorialBox = shopController.ShopScreen.TutorialBox;
            TutorialPointer.ShowFadePointer(tutorialBox.transform as RectTransform, size: 400f);

            tutorialBox.OpenButton.onClick.AddListener(OnBoxWindowOpened);
        }

        private void OnBoxWindowOpened()
        {
            var tutorialBox = shopController.ShopScreen.TutorialBox;
            tutorialBox.OpenButton.onClick.RemoveListener(OnBoxWindowOpened);

            var openBoxButton = shopController.BoxWindow.TutorialOpenButton;
            TutorialPointer.ShowPointer(openBoxButton.transform as RectTransform);

            boxOpenController.OnBoxOpenFinished += OnBoxOpenFinished;
        }

        private void OnBoxOpenFinished()
        {
            boxOpenController.OnBoxOpenFinished -= OnBoxOpenFinished;

            var cardsTabButton = menuController.Tabs.GetTabButton(TabType.Cards);
            TutorialPointer.ShowPointer(cardsTabButton.transform as RectTransform);

            cardsTabButton.Button.onClick.AddListener(OnCardsScreenOpened);
        }



        private void OnCardsScreenOpened()
        {
            var cardsTabButton = menuController.Tabs.GetTabButton(TabType.Cards);
            cardsTabButton.Button.onClick.RemoveListener(OnCardsScreenOpened);

            var card = cardsController.CardsScreen.GetInventoryCard(CardType.CoinsMultiplier);
            TutorialPointer.ShowPointer(card.transform as RectTransform);

            card.OpenButton.onClick.AddListener(OnCardInfoWindowOpened);
        }

        private void OnCardInfoWindowOpened()
        {
            var card = cardsController.CardsScreen.GetInventoryCard(CardType.CoinsMultiplier);
            card.OpenButton.onClick.RemoveListener(OnCardInfoWindowOpened);

            var upgradeButton = cardsController.CardInfoWindow.UpgradeButton;

            TutorialPointer.ShowPointer(upgradeButton.transform as RectTransform);

            upgradeButton.onClick.AddListener(OnCardUpgraded);
        }

        public void OnCardUpgraded()
        {
            var upgradeButton = cardsController.CardInfoWindow.UpgradeButton;
            upgradeButton.onClick.RemoveListener(OnCardUpgraded);

            var takeButton = cardsController.CardInfoWindow.TakeButton;

            TutorialPointer.ShowPointer(takeButton.transform as RectTransform);

            takeButton.onClick.AddListener(OnCardTaken);
        }

        public void OnCardTaken()
        {
            var takeButton = cardsController.CardInfoWindow.TakeButton;
            takeButton.onClick.RemoveListener(OnCardTaken);

            var locationsTabButton = menuController.Tabs.GetTabButton(TabType.Locations);
            TutorialPointer.ShowPointer(locationsTabButton.transform as RectTransform);

            IGameState.

            locationsTabButton.Button.onClick.AddListener(OnLocationsScreenOpened);
        }

        private void OnLocationsScreenOpened()
        {
            var locationsTabButton = menuController.Tabs.GetTabButton(TabType.Locations);
            locationsTabButton.Button.onClick.RemoveListener(OnLocationsScreenOpened);

            // Показываем указатель на кнопку "Играть" в экране локаций
            var playButton = menuController.LocationsScreen.PlayButton;

            TutorialPointer.ShowPointer(playButton.transform as RectTransform);

            playButton.onClick.AddListener(OnPlayButtonClick);
        }

        private void OnPlayButtonClick() => TutorialPointer.HidePointer();

    }
}
