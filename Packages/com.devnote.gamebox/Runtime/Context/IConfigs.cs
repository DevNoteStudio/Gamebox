using UnityEngine;

namespace Gamebox
{
    public interface IConfigs
    {
        public static GameboxConfig Gamebox => Resources.Load<GameboxConfig>("- Gamebox -");

        public static LeadersConfig Leaders => Resources.Load<LeadersConfig>("Leaders");
        public static InternalConfig Internal => Resources.Load<InternalConfig>("Internal");

        public static T GetViewPrefab<T>() where T : Component
            => Resources.Load<T>($"Views/{typeof(T).Name.Replace("View", string.Empty)}");


    }
}

