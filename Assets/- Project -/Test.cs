using DevNote;
using Gamebox;
using UnityEngine;
using IGameState = Gamebox.IGameState;

public class Test : MonoBehaviour
{
    [SerializeField] private LeagueType _leagueType;


    private readonly Holder<LeagueController> leagueController = new();
    private readonly Holder<RollupController> rollupController = new();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            IGameState.Rating.Value += 100;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            IGameState.Rating.Value += 1000;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            IGameState.Rating.Value += 10000;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            IGameState.Rating.Value += 100000;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            leagueController.Item.ShowLeagueLevelUpScreen(_leagueType);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            int add = 50;
            IGameState.Items.Add(ItemKey.Coins, add);
            rollupController.Item.RollupCoins(add);
        }

    }

}
