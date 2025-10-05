using UnityEngine;

namespace Gamebox
{
    public interface IConfigs
    {
        public static GameboxConfig Gamebox => Resources.Load<GameboxConfig>("- Gamebox -");

    }
}

