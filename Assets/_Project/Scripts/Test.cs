using DevNote;
using DevNote.Modules.Levels;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

public class Test : MonoBehaviour
{
    [Inject] private readonly ScreenController screenController;
    [Inject] private readonly LevelController levelController;

    private void Start()
    {
        screenController.ShowLocationsScreen(GameState.LevelProgress.CurrentLocationIndex);
    }


    [Button("Star 1")]
    private void Complete1()
    {
        levelController.CompleteCurrentLevel(1);
        screenController.ShowLocationsScreen(GameState.LevelProgress.CurrentLocationIndex);
    }

    [Button("Star 2")]
    private void Complete2()
    {
        levelController.CompleteCurrentLevel(2);
        screenController.ShowLocationsScreen(GameState.LevelProgress.CurrentLocationIndex);
    }

    [Button("Star 3")]
    private void Complete3()
    {
        levelController.CompleteCurrentLevel(3);
        screenController.ShowLocationsScreen(GameState.LevelProgress.CurrentLocationIndex);
    }


}
