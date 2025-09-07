using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace DevNote.Gamebox
{
    public class ScaleAnimation : MonoBehaviour
    {
        [SerializeField, Expandable] private ScaleAnimationPreset _preset;


        private void OnEnable()
        {
            transform.localScale = Vector3.one * _preset.FromToScale.x;

            DOTween.Sequence().Attach(gameObject)
                .Append(transform.DOScale(_preset.FromToScale.y, _preset.LoopDuration / 2f).SetEase(Ease.OutFlash))
                .Append(transform.DOScale(_preset.FromToScale.x, _preset.LoopDuration / 2f).SetEase(Ease.InFlash))
                .SetLoops(-1, LoopType.Restart);
        }
    }
}

