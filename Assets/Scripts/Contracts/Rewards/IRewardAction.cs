using UnityEngine;

namespace WheelGame.Contracts.Rewards
{
    /// <summary>
    /// Contract for all rewards generated on the wheel.
    /// Combines the visual representation with the execution logic.
    /// </summary>
    public interface IRewardAction
    {
        /// <summary>
        /// The visual sprite displayed on the wheel slice.
        /// </summary>
        Sprite Icon { get; }

        RewardType Type { get; }
    }
}