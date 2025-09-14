using System;

namespace DevNote.Gamebox
{
    public partial class GameboxConfig // Test
    {
        [Serializable] private struct TestResources
        {
            public bool testEnabled;
            public TestLevelView testLevelPrefab;
        }

        public bool TestEnabled => _testResources.testEnabled;

        public TestLevelView TestLevelPrefab => _testResources.testLevelPrefab;

    }
}

