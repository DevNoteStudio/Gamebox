using UnityEngine;

namespace DevNote.Gamebox
{
    public class UI
    {
        public static RectTransform Container { get; private set; }

        public readonly static Vector2 TARGET_RESOLUTION = new Vector2(1080, 1920);

        public UI(RectTransform container) => Container = container;

    }
}

