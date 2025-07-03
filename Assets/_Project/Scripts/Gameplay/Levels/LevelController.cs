using System;
using DevNote;
using Zenject;

public class LevelController : IInitializable
{
    public event Action OnLevelStarted;

    void IInitializable.Initialize()
    {
        StartLevel(GameState.CurrentLocation.Value, GameState.CurrentLevel.Value);
    }

    public void StartLevel(int location, int level)
    {
        GameState.CurrentLocation.Value = location;
        GameState.CurrentLevel.Value = level;

        OnLevelStarted?.Invoke();
    }

    public void WinWith(int starCount)
    {
        if (starCount < 1 || starCount > 3)
            throw new ArgumentOutOfRangeException(nameof(starCount), "Star count must be between 1 and 3.");

        GameState.CompletedLevels.Value[GameState.CurrentLocation.Value][GameState.CurrentLevel.Value] = starCount;

        if (Configs.Locations.List[GameState.CurrentLocation.Value].LevelCount - 1 == GameState.CurrentLevel.Value)
            StartLevel(GameState.CurrentLocation.Value + 1, 0);

        else StartLevel(GameState.CurrentLocation.Value, GameState.CurrentLevel.Value + 1);
    }
}
