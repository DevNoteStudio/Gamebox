using System;
using DevNote;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gamebox
{
    public class ItemTutorialWindowView : MonoBehaviour
    {
        [SerializeField] private RectTransform _windowRect;
        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private Button _submitButton;

        private ItemKey _itemKey;
        private Tween _currentTween;

        private readonly Holder<PopupController> popupController = new();


        private void Start()
        {
            _submitButton.onClick.AddListener(OnSubmitButtonClick);
        }


        public ItemTutorialWindowView Display(ItemKey itemKey)
        {
            _itemKey = itemKey;
            _nameText.text = IConfigs.Gamebox.GetItemTutorialName(itemKey);
            _descriptionText.text = IConfigs.Gamebox.GetItemTutorialDescription(itemKey);
            _iconImage.sprite = IConfigs.Gamebox.GetItemIconSprite(itemKey);

            return this;
        }

        public void AnimateShow()
        {
            _currentTween?.Kill();
            _currentTween = DOTween.Sequence().Attach(gameObject)
                .Append(TweenHub.Show(_windowRect, playSound: true));
        }


        public void AnimateHide(Action onCompleted)
        {
            _currentTween?.Kill();
            _currentTween = DOTween.Sequence().Attach(gameObject)
                .Append(TweenHub.Hide(_windowRect))
                .OnComplete(() => onCompleted?.Invoke());
        }


        private void OnSubmitButtonClick()
        {
            IGameState.ItemTutorials.SetCompleted(_itemKey, true);
            popupController.Item.HidePopup(PopupType.ItemTutorial);
        }

    }
}


