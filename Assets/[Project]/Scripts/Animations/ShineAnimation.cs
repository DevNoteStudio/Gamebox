using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DevNote.Gamebox
{
    public class ShineAnimation : MonoBehaviour
    {
        [SerializeField] private Vector2 _fromToAlpha;
        [SerializeField] private float _loopDuration;
        [SerializeField] private List<Image> _images;
        [SerializeField] private List<TextMeshProUGUI> _texts;


        private void OnEnable()
        {
            var sequence = DOTween.Sequence().Attach(gameObject);

            foreach (var image in _images)
            {
                image.color = image.color.SetAlpha(_fromToAlpha.x);
                sequence.Join(image.DOFade(_fromToAlpha.y, _loopDuration / 2f).SetLoops(2, LoopType.Yoyo));
            }

            foreach (var text in _texts)
            {
                text.color = text.color.SetAlpha(_fromToAlpha.x);
                sequence.Join(text.DOFade(_fromToAlpha.y, _loopDuration / 2f).SetLoops(2, LoopType.Yoyo));
            }

            sequence.SetLoops(-1, LoopType.Restart);
        }


    }
}


