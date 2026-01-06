using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gamebox
{
    public class BoxOpenScreenView : MonoBehaviour
    {
        [SerializeField] private Button _interactButton;
        [SerializeField] private GameObject _promptObject;
        [SerializeField] private BoxView _boxView;
        [SerializeField] private CardView _cardView;
        [SerializeField] private BoxCardAnimation _cardAnimation;

        private Dictionary<CardType, int> _generatedCards;
        private bool _boxOpened = false;


        private void Start()
        {
            _interactButton.onClick.AddListener(OnInteractButtonClick);
        }

        public BoxOpenScreenView Display(ItemKey boxItemKey, Dictionary<CardType, int> generatedCards)
        {
            _generatedCards = generatedCards;
            _boxOpened = false;
            _boxView.Display(boxItemKey);

            return this;
        }

        public void AnimateShow()
        {
            _boxView.AnimateShow();
            _cardView.gameObject.SetActive(false);
        }



        private void OnInteractButtonClick()
        {
            if (!_boxOpened)
            {
                _boxView.AnimateOpen();
                _promptObject.SetActive(false);

                _boxOpened = true;
            }
            else
            {
                _boxView.AnimatePushCard();
                _cardView.gameObject.SetActive(true);
                _cardAnimation.AnimateShow(RarityType.Common);
            }
        }


    }
}
