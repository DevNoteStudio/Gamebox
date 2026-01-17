using System.Collections.Generic;
using DevNote;
using UnityEngine;
using UnityEngine.UI;

namespace Gamebox
{
    public class ShopScreenView : MonoBehaviour
    {
        [SerializeField] private Image _topPanelImage;
        [field: SerializeField] public ShopBoxView TutorialBox { get; private set; }
        [SerializeField] private List<GridLayoutGroup> _grids;

        private readonly Vector2 PORTRAIT_CELL_SIZE = new Vector2(330, 400);
        private readonly Vector2 PORTRAIT_CELL_SPACE = new Vector2(15, 50);
        private readonly Vector2 LANDSCAPE_CELL_SIZE = new Vector2(390, 400);
        private readonly Vector2 LANDSCAPE_CELL_SPACE = new Vector2(70, 50);




        private void OnEnable()
        {
            ScreenState.OnOrientationChanged += OnOrientationChanged;
            OnOrientationChanged();
        }

        private void OnDisable()
        {
            ScreenState.OnOrientationChanged -= OnOrientationChanged;
        }

        private void OnOrientationChanged()
        {
            bool isPortrait = ScreenState.Orientation == Orientation.Portrait;

            foreach (var grid in _grids)
            {
                grid.spacing = isPortrait ? PORTRAIT_CELL_SPACE : LANDSCAPE_CELL_SPACE;
                grid.cellSize = isPortrait ? PORTRAIT_CELL_SIZE : LANDSCAPE_CELL_SIZE;
            }

            float alpha = isPortrait ? 1f : 0f;
            _topPanelImage.color = _topPanelImage.color.SetAlpha(alpha);
        }



    }
}
