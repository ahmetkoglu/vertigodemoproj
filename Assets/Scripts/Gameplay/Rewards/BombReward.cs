using UnityEngine;
using WheelGame.Contracts.Rewards;

namespace WheelGame.Gameplay.Rewards
{
[CreateAssetMenu(fileName = "BombData", menuName = "GameData/Rewards/Bomb")]
public class BombReward : ScriptableObject, IRewardAction
{
    public Sprite icon;
    
    // Interface Implementations
    public Sprite Icon => icon;
    public RewardType Type => RewardType.Bomb; // Always returns Bomb
}
}