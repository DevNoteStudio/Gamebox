using UnityEngine;

namespace DevNote.Gamebox
{
    public class UI
    {
        private static RectTransform _container;

        public static RectTransform Container => _container;


        public static Vector2 TARGET_RESOLUTION = new Vector2(1080, 1920);



        public UI(RectTransform container)
        {
            _container = container;
        }


    }
}

