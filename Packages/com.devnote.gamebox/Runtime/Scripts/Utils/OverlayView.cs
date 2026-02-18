using DevNote;
using UnityEngine;
using UnityEngine.UI;

namespace Gamebox
{
    public class OverlayView : MonoBehaviour
    {
        [SerializeField] private Button _resetButton;

        private int _clicksToReset = 10;

        private readonly Holder<ISave> save = new();


        private void Start()
        {
            _resetButton.onClick.AddListener(OnResetButtonClick);
        }


        private void OnResetButtonClick()
        {
            _clicksToReset--;
            if (_clicksToReset > 0) return;


            save.Item.DeleteSaves(onSuccess: () =>
            {
                _resetButton.image.color = Color.red;
            });
        }
    }
}


