using WheelGame.Contracts.Rewards;
using WheelGame.Contracts.StateMachine;

namespace WheelGame.Tests.EditMode.TestDoubles
{
    public class SpyRewardResolver : IRewardResolver
    {
        public bool ResolveCalled { get; private set; }
        public IRewardAction LastReward { get; private set; }
        public IGameContext LastContext { get; private set; }

        public void Resolve(IRewardAction reward, IGameContext context)
        {
            ResolveCalled = true;
            LastReward = reward;
            LastContext = context;
        }
    }
}