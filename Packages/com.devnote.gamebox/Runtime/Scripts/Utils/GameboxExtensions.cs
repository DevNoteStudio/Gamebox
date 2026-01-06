using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DevNote;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Gamebox
{

    public static class GameboxExtensions
    {

        public static async void LoadSprite(this Image image, UniTask<Sprite> loader)
        {
            float alpha = image.color.a;
            image.color = image.color.SetAlpha(0f);
            image.sprite = await loader;
            image.color = image.color.SetAlpha(alpha);
        }

        public static T FindOrException<T>(this List<T> list, Predicate<T> predicate)
        {
            int index = list.FindIndex(predicate);
            if (index == -1) throw new Exception("List doesn't contain the desired value!");
            return list[index];
        }

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


        public static bool IsCoveredByOtherElement(this RectTransform target)
        {
            List<RaycastResult> allResults = new List<RaycastResult>();
            
            foreach (var raycaster in Object.FindObjectsOfType<GraphicRaycaster>())
            {
                Vector2 screenPos = RectTransformUtility.WorldToScreenPoint
                    (raycaster.eventCamera, target.position);

                PointerEventData pointerData = new PointerEventData(EventSystem.current)
                {
                    position = screenPos
                };

                List<RaycastResult> results = new List<RaycastResult>();
                raycaster.Raycast(pointerData, results);
                allResults.AddRange(results);
            }

            allResults.Sort((a, b) =>
            {
                if (a.sortingLayer == b.sortingLayer)
                    return b.sortingOrder.CompareTo(a.sortingOrder);

                return b.sortingLayer.CompareTo(a.sortingLayer);
            });

            return allResults.Count > 0 ? allResults[0].gameObject != target.gameObject : false;
        }



    }
}


