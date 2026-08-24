using WheelGame.Contracts.StateMachine;

namespace WheelGame.Contracts.Rewards
{
    public interface IRewardResolutionHandler
    {
        bool CanHandle(IRewardAction reward);
        void Resolve(IRewardAction reward, IGameContext context);
    }
}