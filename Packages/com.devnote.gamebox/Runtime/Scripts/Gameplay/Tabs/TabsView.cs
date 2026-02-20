using System.Collections.Generic;
using DevNote;
using DG.Tweening;
using UnityEngine;


namespace Gamebox
{
    public class TabsView : MonoBehaviour
    {
        [SerializeField] private List<TabButtonView> _tabButtons;

        private Tween _currentTween;

        private readonly Holder<MenuController> menuController = new();
        private readonly Holder<ShopController> shopController = new();
        private readonly Holder<CardsController> cardsController = new();


        private void Start()
        {
            foreach (var tabButton in _tabButtons)
                tabButton.onClick.AddListener(OnTabButtonClick);

        }

        public TabButtonView GetTabButton(TabType tabType) 
            => _tabButtons.FindOrException(tabButton => tabButton.TabType == tabType);


        public void AnimateSelectTab(TabType tabType)
        {
            foreach (var tabButton in _tabButtons)
            {
                bool selected = tabButton.TabType == tabType;
                tabButton.SetSelected(selected);
            }
        }


        private void OnTabButtonClick(TabButtonView tabButton)
        {
            OnTabSelected(tabButton.TabType);
        }


        private void OnTabSelected(TabType tabType)
        {
            UI.HideLastView();

            switch (tabType)
            {
                case TabType.Locations:
                    menuController.Item.ShowLocationsScreen();
                    break;

                case TabType.Shop:
                    shopController.Item.ShowShopScreen();
                    break;

                case TabType.Cards:
                    cardsController.Item.ShowCardsScreen();
                    break;
            }
        }

    }
}




