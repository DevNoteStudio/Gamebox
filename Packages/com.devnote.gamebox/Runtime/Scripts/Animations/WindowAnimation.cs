using System;
using DG.Tweening;
using Gamebox;
using UnityEngine;

public class WindowAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform _windowRect;
    [SerializeField] private RectTransform _underButtonRect;
    [SerializeField] private CanvasGroup _canvasGroup;

    private Tween _currentTween;

    private float WINDOW_SHOW_DURATION = 0.7f;
    private float UNDER_BUTTON_SHOW_DELAY = 1.5f;

    private void OnDisable() => _currentTween?.Kill();

    public void AnimateShow()
    {
        _canvasGroup.interactable = false;

        _currentTween?.Kill();

        var sequence = DOTween.Sequence()
            .Append(TweenHub.Show(_windowRect, duration: WINDOW_SHOW_DURATION, playSound: true))
            .AppendCallback(() => _canvasGroup.interactable = true);

        if (_underButtonRect != null)
        {
            sequence
                .AppendInterval(UNDER_BUTTON_SHOW_DELAY)
                .Append(TweenHub.Show(_underButtonRect));
        }

        _currentTween = sequence;
    }

    public void AnimateHide(Action onCompleted)
    {
        _canvasGroup.interactable = false;

        _currentTween?.Kill();
        _currentTween = DOTween.Sequence()
            .Append(TweenHub.Hide(_windowRect, playSound: true))
            .OnComplete(() => onCompleted?.Invoke());
    }


}
