using DevNote;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Gamebox
{
    public enum TabType { Locations, Shop, Cards }

    public class TabButtonView : MonoBehaviour
    {
        public readonly UnityEvent<TabButtonView> onClick = new();

        [field: SerializeField] public TabType TabType { get; private set; }
        [SerializeField] private Button _button; public Button Button => _button;
        [SerializeField] private RectTransform _iconRect; public RectTransform IconRect => _iconRect;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private LayoutElement _layoutElement;
        [SerializeField] private GameObject _markerObject;

        public bool Selected { get; private set; } = false;
        private Tween _currentTween;

        private const float SELECTED_PREFERRED_WIDTH = 400;
        private const float ICON_SELECTED_SCALE = 1.4f;
        private const float ICON_SELECTED_LOCAL_Y = 90f;
        private const float ANIMATION_DURATION = 0.3f;


        private void Awake()
        {
            _nameText.gameObject.SetActive(false);
            _markerObject.SetActive(false);
        }


        private void Start() => _button.onClick.AddListener(OnButtonClick);

        private void OnButtonClick()
        {
            Sound.Play(SoundName.TabClick);
            onClick?.Invoke(this);
        }


        public void SetMarker(bool active) => _markerObject.SetActive(active);

        public void SetSelected(bool value)
        {
            if (Selected == value) return;
            Selected = value;

            if (Selected)
            {
                _nameText.gameObject.SetActive(true);
                _nameText.transform.localScale = Vector3.zero;

                float height = _button.image.rectTransform.sizeDelta.y;

                _currentTween?.Kill();
                _currentTween = DOTween.Sequence()
                    .Append(_layoutElement.DOPreferredSize(new Vector2(SELECTED_PREFERRED_WIDTH, height), ANIMATION_DURATION))
                    .Join(_iconRect.DOScale(ICON_SELECTED_SCALE, ANIMATION_DURATION))
                    .Join(_iconRect.DOLocalMoveY(ICON_SELECTED_LOCAL_Y, ANIMATION_DURATION))
                    .Join(_nameText.transform.DOScale(1f, ANIMATION_DURATION))
                    .SetEase(Ease.OutFlash);
            }
            else
            {
                float height = _button.image.rectTransform.sizeDelta.y;

                _currentTween?.Kill();
                _currentTween = DOTween.Sequence()
                    .Append(_layoutElement.DOPreferredSize(new Vector2(0f, height), ANIMATION_DURATION))
                    .Join(_iconRect.DOScale(1f, ANIMATION_DURATION))
                    .Join(_iconRect.DOLocalMoveY(0f, ANIMATION_DURATION))
                    .Join(_nameText.transform.DOScale(0f, ANIMATION_DURATION))
                    .SetEase(Ease.OutFlash)
                    .OnComplete(() => _nameText.gameObject.SetActive(false));
            }
            
        }




    }
}


