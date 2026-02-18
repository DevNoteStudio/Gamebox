using UnityEngine;

namespace Gamebox
{
    public class GameboxRoot : MonoBehaviour
    {
        [field: SerializeField] public Canvas Canvas { get; private set; }
        [field: SerializeField] public RectTransform ScreenContainer { get; private set; }
        [field: SerializeField] public RectTransform FadeContainer { get; private set; }
        [field: SerializeField] public ExtendedGraphicRaycaster GraphicRaycaster { get; private set; }



        public void ConnectCamera(Camera camera)
        {
            Canvas.worldCamera = camera;
            Canvas.planeDistance = 0.01f;
            Canvas.sortingLayerName = "UI2";
            Canvas.sortingOrder = 0;
        }


    }
}
