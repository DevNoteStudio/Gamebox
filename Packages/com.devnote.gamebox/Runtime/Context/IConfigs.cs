using UnityEngine;

namespace Gamebox
{
    public interface IConfigs
    {
        public static GameboxConfig Gamebox => Resources.Load<GameboxConfig>("- Gamebox -");

        public static InternalConfig Internal => Resources.Load<InternalConfig>("Internal");


        

    }
}

