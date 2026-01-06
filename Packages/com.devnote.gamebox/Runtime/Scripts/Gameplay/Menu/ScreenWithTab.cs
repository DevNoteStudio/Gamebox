using Cysharp.Threading.Tasks;
using DevNote;
using UnityEngine;

namespace Gamebox
{
    public class ScreenWithTab : MonoBehaviour
    {
        [SerializeField] private TabType _tabType;

        private readonly Holder<MenuController> menuController = new();

        private void OnEnable()
        {
            menuController.Item.SetTabsActive(true, _tabType, transform as RectTransform);
        }

    }
}
