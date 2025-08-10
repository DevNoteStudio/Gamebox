using System;


namespace DevNote.LevelUp
{
    public partial class LevelUpConfig // Resources
    {
        [Serializable] private struct ResourcesData
        {
            public LevelCatalogScreenView levelCatalogScreenPrefab;
            public LocationCatalogScreenView locationCatalogScreenPrefab;
        }


        public LevelCatalogScreenView LevelCatalogScreenPrefab => _resources.levelCatalogScreenPrefab;
        public LocationCatalogScreenView LocationCatalogScreenPrefab => _resources.locationCatalogScreenPrefab;





    }

}
