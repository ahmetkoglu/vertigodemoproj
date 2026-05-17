/// <summary>
/// Defines the categories of rewards that can be obtained from the wheel.
/// </summary>
public enum RewardType 
{ 
    Coin, 
    Item, 
    Weapon, 
    Bomb,
    Points,
    Chest
}

/// <summary>
/// Defines the types of progression zones in the game.
/// Dictates the risk level and the type of wheel generated.
/// </summary>
public enum ZoneType 
{ 
    Normal, 
    Safe, 
    Super 
}