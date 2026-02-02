using Cysharp.Threading.Tasks;
using DevNote;
using UnityEngine;
using UnityEngine.UI;


namespace Gamebox
{
    public class TutorialPointer
    {
        public enum PointerType { Up, Down }

        private static Viewer<TutorialPointerView> tutorialPointerViewer;
        private static Button _currentOnlyButton;
        private static float _delayBetweenFadeSteps = 0f;

        public TutorialPointer()
        {
            tutorialPointerViewer = new(Resources.Load<TutorialPointerView>($"Views/TutorialPointer"));
        }

        public static void SetDelayBetweenFadeSteps(float delay) => _delayBetweenFadeSteps = delay;


        public static void ShowPointer(Transform target, PointerType pointerType, Vector2 offset = default)
        {
            var screen = tutorialPointerViewer.ShowExpand(UI.Container);
            screen.AnimatePointer(target, offset, pointerType);
        }

        public static async void ShowFadePointer(Transform target, float viewportSize, 
            PointerType pointerType, Vector2 offset = default)
        {
            if (tutorialPointerViewer.ViewExists)
                tutorialPointerViewer.View.HidePointer();

            await UniTask.WaitForSeconds(_delayBetweenFadeSteps);

            var screen = tutorialPointerViewer.ShowExpand(UI.Container);
            screen.AnimateFadePointer(target, viewportSize, offset, pointerType);
        }

        public static void HidePointer()
        {
            tutorialPointerViewer.View.HidePointer(onCompleted: tutorialPointerViewer.Hide);
        }

    }

}


