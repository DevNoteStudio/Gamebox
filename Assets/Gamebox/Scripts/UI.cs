using UnityEngine;

namespace DevNote.Gamebox
{
    public class UI
    {
        public static RectTransform Container { get; private set; }
        public static RectTransform FadeContainer { get; private set; }

        public readonly static Vector2 TARGET_RESOLUTION = new Vector2(1080, 1920);

        public UI(RectTransform mainContainer, RectTransform fadeContainer)
        {
            Container = mainContainer;
            FadeContainer = fadeContainer;
        }


    }
}

