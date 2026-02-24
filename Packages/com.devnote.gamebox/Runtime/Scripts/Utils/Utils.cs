using UnityEngine;

namespace Gamebox
{
    public static class Utils
    {
        public static Vector2 WorldToCanvas(Vector3 worldPosition, Canvas canvas, Camera camera)
        {
            bool isOverlay = canvas.renderMode == RenderMode.ScreenSpaceOverlay;

            Vector3 screenPosition = camera.WorldToScreenPoint(worldPosition);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rect: canvas.transform as RectTransform,
                screenPoint: screenPosition,
                cam: isOverlay ? null : camera,
                out Vector2 canvasPosition
            );

            return canvasPosition;
        }

    }
}
