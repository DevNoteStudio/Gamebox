
public class EnvironmentKey : DevNote.IEnvironmentKey
{

}

public class ItemKey : Gamebox.IItemKey
{

}



public class AdKey : DevNote.IAdKey, Gamebox.IAdKey
{

}

public class TableKey : DevNote.ITableKey, Gamebox.ITableKey
{

}

public class LeaderboardKey : DevNote.ILeaderboardKey
{
    
}

public class ProductKey : DevNote.IProductKey
{
    public const string NoAds = DevNote.IProductKey.NoAds;

}

