using System.Collections.Generic;
using DevNote;
using UnityEngine;

namespace Gamebox
{
    public class ScreenWithCurrencyWidget : MonoBehaviour
    {
        private static List<CurrencyWidgetView> _activeWidgets = new();


        [SerializeField] private bool _moreButtonActive;

        private Transform _previousParent;

        private Viewer<CurrencyWidgetView> currencyWidgetViewer;
        


        private void OnEnable()
        {
            if (currencyWidgetViewer == null)
                currencyWidgetViewer = new(IConfigs.GetViewPrefab<CurrencyWidgetView>());

            if (currencyWidgetViewer.ViewExists)
                _previousParent = currencyWidgetViewer.View.transform.parent;

            currencyWidgetViewer.ShowExpand(transform as RectTransform)
                .SetMoreButtonActive(_moreButtonActive);

            if (_activeWidgets.Count > 0)
                _activeWidgets[^1].gameObject.SetActive(false);

            _activeWidgets.Add(currencyWidgetViewer.View);
        }

        private void OnDisable()
        {
            _activeWidgets.Remove(currencyWidgetViewer.View);

            if (_activeWidgets.Count > 0)
                _activeWidgets[^1].gameObject.SetActive(true);
        }

    }
}
