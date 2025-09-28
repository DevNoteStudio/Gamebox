using DG.Tweening;
using UnityEngine;

namespace Gamebox
{
    public static class GameboxExtensions
    {
        
        public static T Attach<T>(this T tween, GameObject target) where T : Tween
            => tween.SetLink(target, LinkBehaviour.KillOnDisable);



    }
}


