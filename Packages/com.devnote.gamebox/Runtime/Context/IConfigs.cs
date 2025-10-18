using UnityEngine;

namespace Gamebox
{
    public interface IConfigs
    {
        public static GameboxConfig Gamebox => Resources.Load<GameboxConfig>("- Gamebox -");

        public static T GetViewPrefab<T>() where T : Component 
            => Resources.Load<T>($"Views/{typeof(T).Name.Replace("View", string.Empty)}");

    }
}

