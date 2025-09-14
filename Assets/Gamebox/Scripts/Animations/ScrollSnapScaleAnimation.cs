using DanielLochner.Assets.SimpleScrollSnap;
using DG.Tweening;
using UnityEngine;


namespace DevNote.Gamebox
{

    [RequireComponent(typeof(SimpleScrollSnap))]
    public class ScrollSnapScaleAnimation : MonoBehaviour
    {
        [SerializeField] private float _selectedScale = 1f;
        [SerializeField] private float _nonSelectedScale = 0.6f;
        [SerializeField] private float _switchDuration = 0.7f;

        private SimpleScrollSnap _scrollSnap;

        private Sequence _sequence;

        private void Awake()
        {
            _scrollSnap = GetComponent<SimpleScrollSnap>();
        }

        private void Start()
        {
            _scrollSnap.OnPanelSelected.AddListener(OnPanelSelected);
            ApplyCurrentScale();
        }

        private void OnPanelSelected(int fromIndex)
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();

            int selectedIndex = _scrollSnap.CenteredPanel;

            for (int i = 0; i < _scrollSnap.Panels.Length; i++)
            {
                float scale = i == selectedIndex ? _selectedScale : _nonSelectedScale;
                _sequence.Join(_scrollSnap.Panels[i].DOScale(scale, _switchDuration));
            }

            _sequence.SetEase(Ease.OutFlash);
        }

        private void ApplyCurrentScale()
        {
            int selectedIndex = _scrollSnap.CenteredPanel;

            for (int i = 0; i < _scrollSnap.Panels.Length; i++)
            {
                float scale = i == selectedIndex ? _selectedScale : _nonSelectedScale;
                _scrollSnap.Panels[i].transform.localScale = Vector3.one * scale;
            }
        }

    }
}

