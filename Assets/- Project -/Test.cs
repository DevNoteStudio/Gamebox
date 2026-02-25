using DevNote;
using Gamebox;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private Transform _worldEmitter;


    private readonly Holder<ScoreController> scoreController = new();
    private readonly Holder<BoosterController> boosterController = new();


    private void Awake()
    {
        scoreController.Item.SetScoreCompletedLevelStars(() => 3);
        boosterController.Item.OnBoosterUsingStarted += OnBoosterUsingStarted;
    }

    private void OnBoosterUsingStarted()
    {
        boosterController.Item.FinishBoosterUsing(success: true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            scoreController.Item.AddParticleScore(2, 5, _worldEmitter.position);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            scoreController.Item.SetScoreRequire(10);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Gamebox.IGameState.Items.Add(ItemKey.Booster1, 1);
        }

    }

}
