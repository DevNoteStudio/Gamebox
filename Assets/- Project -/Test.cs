using DevNote;
using Gamebox;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private Transform _worldEmitter;


    private readonly Holder<ScoreController> scoreController = new();


    private void Awake()
    {
        scoreController.Item.SetScoreCompletedLevelStars(() => 3);
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


    }

}
