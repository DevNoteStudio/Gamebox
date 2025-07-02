using DevNote;

public class LevelController : Zenject.IInitializable
{
    public LevelController() { }

    public event System.Action LevelStarted;

    public void Initialize()
    {
        StartLevel(GameState.CurrentLocation.Value, GameState.CurrentLevel.Value);
    }

    public void StartLevel(int location, int level)
    {
        GameState.CurrentLocation.Value = location;
        GameState.CurrentLevel.Value = level;

        LevelStarted?.Invoke();
    }

    public void WinWith(int starCount)
    {
        if (starCount < 1 || starCount > 3)
            throw new System.ArgumentOutOfRangeException(nameof(starCount), "Star count must be between 1 and 3.");

        GameState.CompletedLevels.Value[GameState.CurrentLocation.Value][GameState.CurrentLevel.Value] = starCount;

        if (Configs.Locations.List[GameState.CurrentLocation.Value].LevelCount - 1 == GameState.CurrentLevel.Value)
            StartLevel(GameState.CurrentLocation.Value + 1, 0);
        else
            StartLevel(GameState.CurrentLocation.Value, GameState.CurrentLevel.Value + 1);
    }
}
