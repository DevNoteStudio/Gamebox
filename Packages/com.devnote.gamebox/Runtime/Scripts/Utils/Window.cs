using System;
using UnityEngine;

namespace Gamebox
{
    [RequireComponent(typeof(WindowAnimation))]
    public class Window : MonoBehaviour
    {
        private WindowAnimation _windowAnimation;


        protected virtual void Awake()
        {
            _windowAnimation = GetComponent<WindowAnimation>();
        }

        public virtual void AnimateShow() => _windowAnimation.AnimateShow();

        public virtual void AnimateHide(Action onCompleted) => _windowAnimation.AnimateHide(onCompleted);


    }

}

