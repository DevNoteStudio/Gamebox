using System.Collections.Generic;
using UnityEngine;

namespace Gamebox
{
    public class CurrencyWidgetView : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _moreButtonObjects;


        public void SetMoreButtonActive(bool value)
            => _moreButtonObjects.ForEach(o => o.SetActive(value));


    }
}
