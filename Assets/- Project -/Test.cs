using DevNote;
using Gamebox;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private Transform _worldEmitter;

    private readonly Holder<LevelController> levelController = new();
    private readonly Holder<ScoreController> scoreController = new();
    private readonly Holder<BoosterController> boosterController = new();
    private readonly Holder<CurrencyController> currencyController = new();


    private void Awake()
    {
        scoreController.Item.SetScoreCompletedLevelStars(() => 3);
        boosterController.Item.OnBoosterUsingStarted += OnBoosterUsingStarted;
        levelController.Item.OnLevelStarted += OnLevelStarted;
    }

    private void OnLevelStarted()
    {
        scoreController.Item.SetScoreRequire((Gamebox.IGameState.Levels.CompletedLevels + 1) * 10);
    }

    private void OnBoosterUsingStarted()
    {
        if (boosterController.Item.CurrentUsingBoosterKey == ItemKey.Booster1)
            boosterController.Item.FinishBoosterUsing(success: true);

        if (boosterController.Item.CurrentUsingBoosterKey == ItemKey.Booster2)
        {
            boosterController.Item.ShowBoosterHint();

        }
            


    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            scoreController.Item.AddParticleScore(2, 5, _worldEmitter.position);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currencyController.Item.AddCoinsRollup(50, 5, _worldEmitter.position);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            boosterController.Item.FinishBoosterUsing(true);
            boosterController.Item.HideBoosterHint();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            levelController.Item.LoseCurrentLevel();
        }

    }

}
