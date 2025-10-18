using System.Collections.Generic;

namespace Gamebox
{
    public delegate bool PopupShowCondition();

    public enum PopupType { ItemTutorial, RateUs, NoAds }

    public struct PopupData
    {
        public PopupType popupType;
        public PopupShowCondition showCondition;
        public int priority;
    }

    public class PriorityPopupList
    { 
        private List<PopupData> _popupDataList;
        private List<(PopupType, int)> _priorityQueue = new();

        public PriorityPopupList(List<PopupData> popupDataList) 
        { 
            _popupDataList = popupDataList;
        }


        public bool TryGetNextPopup(out PopupType popupType)
        {
            popupType = default;

            CheckConditionsAndTryAddToQueue();

            if (_priorityQueue.Count != 0)
            {
                popupType = ExtractHighestPriorityPopup();
                return true;
            }

            else return false;
        }


        private void CheckConditionsAndTryAddToQueue()
        {
            foreach (var popupData in _popupDataList)
            {
                bool needShow = popupData.showCondition.Invoke();
                bool existsInsideQueue = _priorityQueue.Exists((pair) => popupData.popupType == pair.Item1);

                if (needShow && !existsInsideQueue)
                    _priorityQueue.Add((popupData.popupType, popupData.priority));
            }
        }

        private PopupType ExtractHighestPriorityPopup()
        {
            int priority = int.MaxValue;
            PopupType popupType = default;

            foreach (var popupPriority in _priorityQueue)
            {
                if (popupPriority.Item2 < priority)
                {
                    priority = popupPriority.Item2;
                    popupType = popupPriority.Item1;
                }
            }

            _priorityQueue.Remove((popupType, priority));

            return popupType;
        }




    }





}

