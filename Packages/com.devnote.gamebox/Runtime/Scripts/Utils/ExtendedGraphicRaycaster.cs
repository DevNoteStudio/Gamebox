using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

namespace Gamebox
{
    public class ExtendedGraphicRaycaster : GraphicRaycaster
    {

        private GameObject _allowedObject;
        private bool _enabled = false;

        public async void SetAllowObject(GameObject gameObject, float delay = 0f)
        {
            _enabled = true;
            _allowedObject = null;

            await UniTask.WaitForSeconds(delay);

            _allowedObject = gameObject;
        }

        public void DisableRaycastBlock() => _enabled = false;


        public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
        {
            base.Raycast(eventData, resultAppendList);

            if (_enabled)
            {
                if (_allowedObject != null)
                {
                    resultAppendList.RemoveAll(r =>
                        r.gameObject != _allowedObject &&
                        !r.gameObject.transform.IsChildOf(_allowedObject.transform)
                    );
                }
                else resultAppendList.Clear();
            }
        }


    }
}
