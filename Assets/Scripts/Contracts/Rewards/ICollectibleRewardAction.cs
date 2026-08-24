namespace WheelGame.Contracts.Rewards
{
    public interface ICollectibleRewardAction : IRewardAction
    {
        string ItemId { get; }
        int Amount { get; }
    }
}