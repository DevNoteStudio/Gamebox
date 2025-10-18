using System;
using DevNote;
using DG.Tweening;
using UnityEngine;

namespace Gamebox
{

    public static class GameboxExtensions
    {

        public static T Attach<T>(this T tween, GameObject target) where T : Tween
            => tween.SetLink(target, LinkBehaviour.KillOnDisable);


        public static T ShowWindow<T>(this Viewer<T> viewer, RectTransform container) where T : Component
        {
            UI.AddWindowFadePoint();
            return viewer.ShowExpand(container);
        }

        public static void ForceHideWindow<T>(this Viewer<T> viewer) where T : Component
        {
            UI.RemoveWindowFadePoint(forceHide: true);
            viewer.Hide();
        }

        public static void AnimateHideWindow<T>(this Viewer<T> viewer, Action<Action> animation) where T : Component
        {
            UI.RemoveWindowFadePoint();
            animation.Invoke(() => viewer.Hide());
        }


    }
}


