using System.Collections.Generic;
using DevNote;
using DevNote.Gamebox;
using UnityEngine;

public class TestController : IUpdateHandler
{
    private VictoryScreenView _victoryScreen;
    private LoseWindowView _loseWindow;

    public TestController(VictoryScreenView victoryScreen, LoseWindowView loseWindow)
    {
        _victoryScreen = victoryScreen;
        _loseWindow = loseWindow;
    }


    void IUpdateHandler.Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _victoryScreen.gameObject.SetActive(true);

            var rewards = new List<(ItemType, int)>
            {
                (ItemType.Coins, 100),
                (ItemType.Stars, 3)
            };

            _victoryScreen.Display(stars: 3, rewards, showBonus: true);

            _victoryScreen.AnimateShow();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _loseWindow.Display(showRevive: false);
            _loseWindow.AnimateShow();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _loseWindow.Display(showRevive: true);
            _loseWindow.AnimateShow();
        }

    }


}
