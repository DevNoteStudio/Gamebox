using System.Collections.Generic;
using DevNote;
using UnityEngine;

namespace Gamebox
{


    public class BoosterPanelView : MonoBehaviour
    {
        [SerializeField] private BoosterButtonView _boosterButtonPrefab;
        [SerializeField] private RectTransform _portraitContainer;
        [SerializeField] private RectTransform _landscapeContainer;

        private List<BoosterButtonView> _boosterButtons;

        private readonly Holder<LevelController> levelController = new();
        private readonly Holder<BoosterController> boosterController = new();


        private void OnEnable()
        {
            if (_boosterButtons == null) CreateBoosterButtons();

            ScreenState.OnOrientationChanged += OnOrientationChanged;
            levelController.Item.OnLevelStarted += Display;

            foreach (var boosterItemKey in IConfigs.Gamebox.GetAllBoosterTypes())
                IGameState.Items.Subscribe(boosterItemKey, Display);

            OnOrientationChanged();
            Display();
        }

        private void OnDisable()
        {
            ScreenState.OnOrientationChanged -= OnOrientationChanged;
            levelController.Item.OnLevelStarted -= Display;

            foreach (var boosterItemKey in IConfigs.Gamebox.GetAllBoosterTypes())
                IGameState.Items.Dispose(boosterItemKey, Display);
        }

        private void OnOrientationChanged()
        {
            var container = ScreenState.Orientation == Orientation.Portrait ? 
                _portraitContainer : _landscapeContainer;

            foreach (var boosterButton in _boosterButtons)
            {
                boosterButton.transform.SetParent(container, worldPositionStays: false);
            }
                
        }



        private void Display()
        {
            var boosterItemKeys = IConfigs.Gamebox.GetAllBoosterTypes();

            for (int i = 0; i < boosterItemKeys.Count; i++)
                _boosterButtons[i].Display(boosterItemKeys[i], boosterNumber: i + 1);
        }


        private void CreateBoosterButtons()
        {
            _boosterButtons = new();

            foreach (var itemKey in IConfigs.Gamebox.GetAllBoosterTypes())
            {
                var boosterButton = Instantiate(_boosterButtonPrefab, _portraitContainer);
                _boosterButtons.Add(boosterButton);
            }
                
        }




    }
}
