using System;


namespace DevNote.Gamebox
{
    public partial class LevelUpConfig // Resources
    {
        [Serializable] private struct ResourcesData
        {
            public LevelCatalogScreenView levelCatalogScreenPrefab;
            public LocationsScreenView locationCatalogScreenPrefab;
        }


        public LevelCatalogScreenView LevelCatalogScreenPrefab => _resources.levelCatalogScreenPrefab;
        public LocationsScreenView LocationCatalogScreenPrefab => _resources.locationCatalogScreenPrefab;





    }

}
