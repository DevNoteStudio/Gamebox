using DG.Tweening;
using UnityEngine;

namespace DevNote.Gamebox
{
    public static class GameboxExtensions
    {
        
        public static Sequence Attach(this Sequence sequence, GameObject gameObject)
        {
            sequence.SetLink(gameObject, LinkBehaviour.KillOnDisable);
            return sequence;
        }




    }
}


