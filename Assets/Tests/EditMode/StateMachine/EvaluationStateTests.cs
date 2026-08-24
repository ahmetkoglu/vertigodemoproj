using NUnit.Framework;
using UnityEngine.TestTools;
using WheelGame.Contracts.Rewards;
using WheelGame.Gameplay.StateMachine.States;
using WheelGame.Tests.EditMode.TestDoubles;

namespace WheelGame.Tests.EditMode.StateMachine
{
    public class EvaluationStateTests
    {
        private FakeGameContext _context;
        private FakeZoneService _zoneService;
        private SpyRewardResolver _rewardResolver;

        [SetUp]
        public void SetUp()
        {
            _zoneService = new FakeZoneService();
            _rewardResolver = new SpyRewardResolver();
            _context = new FakeGameContext
            {
                Zone = _zoneService,
                RewardResolver = _rewardResolver
            };
        }

        [Test]
        public void EnterState_ShouldTransitionToInit_WhenRewardIsNull()
        {
            _zoneService.RewardAtIndex = null;
            EvaluationState state = new EvaluationState(0);

            LogAssert.Expect(UnityEngine.LogType.Warning, "[EvaluationState] Won reward is null! Aborting and forcing InitState.");

            state.EnterState(_context);

            Assert.IsInstanceOf<InitState>(_context.LastChangedState);
            Assert.IsFalse(_rewardResolver.ResolveCalled);
        }

        [Test]
        public void EnterState_ShouldCallRewardResolver_WhenRewardExists()
        {
            FakeRewardAction reward = new FakeRewardAction(RewardType.Coin);
            _zoneService.RewardAtIndex = reward;
            EvaluationState state = new EvaluationState(2);

            state.EnterState(_context);

            Assert.IsTrue(_rewardResolver.ResolveCalled);
            Assert.AreSame(reward, _rewardResolver.LastReward);
            Assert.AreSame(_context, _rewardResolver.LastContext);
        }

        [Test]
        public void ExitState_ShouldNotThrow()
        {
            EvaluationState state = new EvaluationState(1);

            Assert.DoesNotThrow(() => state.ExitState(_context));
        }

        private sealed class FakeRewardAction : IRewardAction
        {
            public FakeRewardAction(RewardType type)
            {
                Type = type;
            }

            public UnityEngine.Sprite Icon => null;
            public RewardType Type { get; }
        }
    }
}