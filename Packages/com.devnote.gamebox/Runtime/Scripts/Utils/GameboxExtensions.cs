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


        public static T ShowFaded<T>(this Viewer<T> viewer, RectTransform container) where T : Component
        {
            UI.AddFadePoint();
            return viewer.ShowExpand(container);
        }

        public static void ForceFadedHide<T>(this Viewer<T> viewer) where T : Component
        {
            UI.RemoveFadePoint(forceHide: true);
            viewer.Hide();
        }

        public static void AnimateFadedHide<T>(this Viewer<T> viewer, Action<Action> animation) where T : Component
        {
            UI.RemoveFadePoint();
            animation.Invoke(() => viewer.Hide());
        }


    }
}


