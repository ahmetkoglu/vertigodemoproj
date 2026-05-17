using UnityEngine;

[CreateAssetMenu(fileName = "NewCollectible", menuName = "GameData/Rewards/Collectible")]
public class CollectibleReward : ScriptableObject, IRewardAction
{
    [Header("Item Details")]
    public string itemId; 
    public int amount;
    public Sprite icon;
    [SerializeField] private RewardType rewardType = RewardType.Coin;

    // Interface Implementations
    public Sprite Icon => icon;
    public RewardType Type => rewardType; // Returns whether it's a Coin, Cash, or Weapon
}