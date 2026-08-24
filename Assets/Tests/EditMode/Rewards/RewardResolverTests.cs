using System;
using NUnit.Framework;
using WheelGame.Contracts.Rewards;
using WheelGame.Gameplay.Rewards.Resolution;
using WheelGame.Tests.EditMode.TestDoubles;

namespace WheelGame.Tests.EditMode.Rewards
{
    public class RewardResolverTests
    {
        private FakeGameContext _context;

        [SetUp]
        public void SetUp()
        {
            _context = new FakeGameContext();
        }

        [Test]
        public void Resolve_ShouldUseBombHandler_WhenRewardTypeIsBomb()
        {
            BombRewardSpyHandler bombHandler = new BombRewardSpyHandler();
            RewardResolver resolver = new RewardResolver(new IRewardResolutionHandler[] { bombHandler });

            resolver.Resolve(new FakeRewardAction(RewardType.Bomb), _context);

            Assert.IsTrue(bombHandler.ResolveCalled);
        }

        [Test]
        public void Resolve_ShouldUseCollectibleHandler_WhenRewardTypeMatchesCollectibleHandler()
        {
            CollectibleRewardSpyHandler collectibleHandler = new CollectibleRewardSpyHandler();
            RewardResolver resolver = new RewardResolver(new IRewardResolutionHandler[] { collectibleHandler });

            resolver.Resolve(new FakeCollectibleRewardAction(RewardType.Coin), _context);

            Assert.IsTrue(collectibleHandler.ResolveCalled);
        }

        [Test]
        public void Resolve_ShouldUseCollectibleHandler_WhenRewardTypeIsItem()
        {
            CollectibleRewardSpyHandler collectibleHandler = new CollectibleRewardSpyHandler();
            RewardResolver resolver = new RewardResolver(new IRewardResolutionHandler[] { collectibleHandler });

            resolver.Resolve(new FakeCollectibleRewardAction(RewardType.Item), _context);

            Assert.IsTrue(collectibleHandler.ResolveCalled);
        }

        [Test]
        public void Resolve_ShouldThrow_WhenNoHandlerExistsForRewardType()
        {
            RewardResolver resolver = new RewardResolver(Array.Empty<IRewardResolutionHandler>());

            Assert.Throws<InvalidOperationException>(() => resolver.Resolve(new FakeRewardAction(RewardType.Bomb), _context));
        }

        [Test]
        public void Resolve_ShouldThrow_WhenRewardIsNull()
        {
            RewardResolver resolver = new RewardResolver(Array.Empty<IRewardResolutionHandler>());

            Assert.Throws<ArgumentNullException>(() => resolver.Resolve(null, _context));
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

        private sealed class BombRewardSpyHandler : IRewardResolutionHandler
        {
            public bool ResolveCalled { get; private set; }

            public bool CanHandle(IRewardAction reward)
            {
                return reward != null && reward.Type == RewardType.Bomb;
            }

            public void Resolve(IRewardAction reward, WheelGame.Contracts.StateMachine.IGameContext context)
            {
                ResolveCalled = true;
            }
        }

        private sealed class CollectibleRewardSpyHandler : IRewardResolutionHandler
        {
            public bool ResolveCalled { get; private set; }

            public bool CanHandle(IRewardAction reward)
            {
                return reward is ICollectibleRewardAction && reward.Type != RewardType.Bomb;
            }

            public void Resolve(IRewardAction reward, WheelGame.Contracts.StateMachine.IGameContext context)
            {
                ResolveCalled = true;
            }
        }

        private sealed class FakeCollectibleRewardAction : ICollectibleRewardAction
        {
            public FakeCollectibleRewardAction(RewardType type)
            {
                Type = type;
            }

            public UnityEngine.Sprite Icon => null;
            public RewardType Type { get; }
            public string ItemId => "item_01";
            public int Amount => 1;
        }
    }
}