using WheelGame.Contracts.StateMachine;

namespace WheelGame.Contracts.Rewards
{
    public interface IRewardResolver
    {
        void Resolve(IRewardAction reward, IGameContext context);
    }
}