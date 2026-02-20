using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Gamebox
{

    public class BoxAndCardTutorialController
    {
        private ExtendedGraphicRaycaster _graphicRaycaster;

        private readonly MenuController menuController;
        private readonly ShopController shopController;
        private readonly BoxOpenController boxOpenController;
        private readonly CardsController cardsController;

        private const float DELAY_BETWEEN_STEPS = 1f;

        public BoxAndCardTutorialController(MenuController menuController, ShopController shopController,
            BoxOpenController boxOpenController, CardsController cardsController, ExtendedGraphicRaycaster graphicRaycaster)
        {
            _graphicRaycaster = graphicRaycaster;

            this.menuController = menuController;
            this.shopController = shopController;
            this.boxOpenController = boxOpenController;
            this.cardsController = cardsController;

            menuController.OnLocationScreenOpened += OnLocationScreenOpened;
        }

        private void OnLocationScreenOpened()
        {
            if (!IGameState.BoxAndCardTutorialCompleted.Value)
                StartTutorial();
        }

        public void StartTutorial()
        {
            TutorialPointer.SetDelayBetweenFadeSteps(DELAY_BETWEEN_STEPS);

            if (IGameState.Items.Get(ItemKey.CommonBox) == 0)
                IGameState.Items.Add(ItemKey.CommonBox, 1);

            int boxPrice = IConfigs.Gamebox.GetBoxPrice(ItemKey.CommonBox, out _);
            if (IGameState.Items.Get(ItemKey.Coins) < boxPrice)
                IGameState.Items.Set(ItemKey.Coins, boxPrice);

            IGameState.Cards.SetCardToCell(0, CardType.Empty);
            IGameState.Cards.ResetCard(CardType.CoinsMultiplier);

            var shopTabButton = menuController.Tabs.GetTabButton(TabType.Shop);
            TutorialPointer.ShowFadePointer(shopTabButton.transform as RectTransform,
                viewportSize: 500f, TutorialPointer.PointerType.Up, offset: Vector2.up * 80f);
            
            shopTabButton.Button.onClick.AddListener(OnShopScreenOpened);

            _graphicRaycaster.SetAllowObject(shopTabButton.gameObject, DELAY_BETWEEN_STEPS);
        }

        private async void OnShopScreenOpened()
        {
            await UniTask.NextFrame();

            var shopTabButton = menuController.Tabs.GetTabButton(TabType.Shop);
            shopTabButton.Button.onClick.RemoveListener(OnShopScreenOpened);

            shopController.ShopScreen.ScrollRect.enabled = false;

            var tutorialBox = shopController.ShopScreen.TutorialBox;
            TutorialPointer.ShowFadePointer(tutorialBox.transform as RectTransform, 
                viewportSize: 700f, TutorialPointer.PointerType.Down, offset: Vector2.down * 200f);
            
            tutorialBox.OpenButton.onClick.AddListener(OnBoxWindowOpened);
            _graphicRaycaster.SetAllowObject(tutorialBox.gameObject, DELAY_BETWEEN_STEPS);
        }

        private async void OnBoxWindowOpened()
        {
            await UniTask.NextFrame();

            shopController.ShopScreen.ScrollRect.enabled = true;

            var tutorialBox = shopController.ShopScreen.TutorialBox;
            tutorialBox.OpenButton.onClick.RemoveListener(OnBoxWindowOpened);

            var openBoxButton = shopController.BoxWindow.TutorialOpenButton;
            TutorialPointer.ShowPointer(openBoxButton.transform as RectTransform, 
                TutorialPointer.PointerType.Down, offset: Vector2.down * 70f);

            boxOpenController.OnBoxOpenFinished += OnBoxOpenFinished;
            _graphicRaycaster.SetAllowObject(openBoxButton.gameObject, DELAY_BETWEEN_STEPS);

            boxOpenController.SetNextBoxReward(new BoxRewardData
            {
                cards = new Dictionary<CardType, int> { { CardType.CoinsMultiplier, 5 } },
                boosters = new Dictionary<ItemKey, int> { { IConfigs.Gamebox.GetAllBoosterTypes()[0], 3 } },
            });

            openBoxButton.onClick.AddListener(OnOpenBoxButtonClick);
        }

        private void OnOpenBoxButtonClick()
        {
            var openBoxButton = shopController.BoxWindow.TutorialOpenButton;
            openBoxButton.onClick.RemoveListener(OnOpenBoxButtonClick);

            _graphicRaycaster.DisableRaycastBlock();
        }

        private async void OnBoxOpenFinished()
        {
            await UniTask.NextFrame();

            boxOpenController.OnBoxOpenFinished -= OnBoxOpenFinished;

            var cardsTabButton = menuController.Tabs.GetTabButton(TabType.Cards);
            TutorialPointer.ShowFadePointer(cardsTabButton.transform as RectTransform,
                viewportSize: 500f, TutorialPointer.PointerType.Up, offset: Vector2.up * 80f);

            cardsTabButton.Button.onClick.AddListener(OnCardsScreenOpened);
            _graphicRaycaster.SetAllowObject(cardsTabButton.gameObject, DELAY_BETWEEN_STEPS);
        }



        private async void OnCardsScreenOpened()
        {
            await UniTask.NextFrame();

            var cardsTabButton = menuController.Tabs.GetTabButton(TabType.Cards);
            cardsTabButton.Button.onClick.RemoveListener(OnCardsScreenOpened);

            cardsController.CardsScreen.ScrollRect.enabled = false;

            var card = cardsController.CardsScreen.GetInventoryCard(CardType.CoinsMultiplier);
            TutorialPointer.ShowFadePointer(card.transform as RectTransform,
                viewportSize: 600f, TutorialPointer.PointerType.Down, offset: Vector2.down * 170f);

            card.OpenButton.onClick.AddListener(OnCardInfoWindowOpened);
            _graphicRaycaster.SetAllowObject(card.gameObject, DELAY_BETWEEN_STEPS);
        }   

        private async void OnCardInfoWindowOpened()
        {
            await UniTask.NextFrame();

            cardsController.CardsScreen.ScrollRect.enabled = true;

            var card = cardsController.CardsScreen.GetInventoryCard(CardType.CoinsMultiplier);
            card.OpenButton.onClick.RemoveListener(OnCardInfoWindowOpened);

            var upgradeButton = cardsController.CardInfoWindow.UpgradeButton;

            TutorialPointer.ShowPointer(upgradeButton.transform as RectTransform, 
                TutorialPointer.PointerType.Down, offset: Vector2.down * 80f);

            upgradeButton.onClick.AddListener(OnCardUpgraded);
            _graphicRaycaster.SetAllowObject(upgradeButton.gameObject, DELAY_BETWEEN_STEPS);
        }

        private async void OnCardUpgraded()
        {
            await UniTask.NextFrame();

            var upgradeButton = cardsController.CardInfoWindow.UpgradeButton;
            upgradeButton.onClick.RemoveListener(OnCardUpgraded);

            var takeButton = cardsController.CardInfoWindow.TakeButton;

            TutorialPointer.ShowPointer(takeButton.transform as RectTransform,
                TutorialPointer.PointerType.Down, offset: Vector2.down * 80f);

            takeButton.onClick.AddListener(OnCardTaken);
            _graphicRaycaster.SetAllowObject(takeButton.gameObject, DELAY_BETWEEN_STEPS);
        }

        private async void OnCardTaken()
        {
            await UniTask.NextFrame();

            var takeButton = cardsController.CardInfoWindow.TakeButton;
            takeButton.onClick.RemoveListener(OnCardTaken);

            var locationsTabButton = menuController.Tabs.GetTabButton(TabType.Locations);
            TutorialPointer.ShowFadePointer(locationsTabButton.transform as RectTransform,
                viewportSize: 500f, TutorialPointer.PointerType.Up, offset: Vector2.up * 80f);

            IGameState.BoxAndCardTutorialCompleted.Value = true;

            locationsTabButton.Button.onClick.AddListener(OnLocationsScreenOpened);
            _graphicRaycaster.SetAllowObject(locationsTabButton.gameObject, DELAY_BETWEEN_STEPS);
        }

        private async void OnLocationsScreenOpened()
        {
            await UniTask.NextFrame();

            var locationsTabButton = menuController.Tabs.GetTabButton(TabType.Locations);
            locationsTabButton.Button.onClick.RemoveListener(OnLocationsScreenOpened);

            var playButton = menuController.LocationsScreen.PlayButton;

            TutorialPointer.ShowPointer(playButton.transform as RectTransform, 
                TutorialPointer.PointerType.Up, offset: Vector2.up * 100f);

            playButton.onClick.AddListener(OnPlayButtonClick);
            _graphicRaycaster.SetAllowObject(playButton.gameObject, DELAY_BETWEEN_STEPS);
        }

        private void OnPlayButtonClick()
        {
            TutorialPointer.HidePointer();
            _graphicRaycaster.DisableRaycastBlock();
        }

    }
}
