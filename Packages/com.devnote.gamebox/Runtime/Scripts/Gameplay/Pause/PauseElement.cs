using DevNote;
using UnityEngine;

namespace Gamebox
{
    public class PauseElement : MonoBehaviour
    {
        private readonly Holder<PauseController> pauseController = new();


        private void OnEnable()
        {
            pauseController.Item.AddPauseGameplay();
        }

        private void OnDisable()
        {
            pauseController.Item.RemovePauseGameplay();
        }



    }
}
