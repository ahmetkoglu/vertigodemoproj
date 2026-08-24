using NUnit.Framework;
using WheelGame.Contracts.Rewards;
using WheelGame.Contracts.Services;
using WheelGame.Contracts.StateMachine;
using WheelGame.Gameplay.Rewards.Resolution;

namespace WheelGame.Tests.PlayMode.Rewards
{
    public class BombRewardResolutionHandlerPlayModeTests
    {
        [Test]
        public void Resolve_ShouldPlayBombTensionAndTransitionToGameOver()
        {
            FakeOverlayUI overlay = new FakeOverlayUI();
            FakeRewardFlowService rewardFlow = new FakeRewardFlowService();

            FakeGameContext context = new FakeGameContext
            {
                OverlayUI = overlay,
                RewardFlow = rewardFlow
            };

            BombRewardResolutionHandler handler = new BombRewardResolutionHandler();

            handler.Resolve(new FakeRewardAction(RewardType.Bomb), context);

            Assert.IsTrue(overlay.BombTensionCalled);
            Assert.IsTrue(rewardFlow.TransitionToGameOverCalled);
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

        private sealed class FakeGameContext : IGameContext
        {
            public IInputControlUI InputUI { get; set; }
            public IInventoryUI InventoryUI { get; set; }
            public IOverlayUI OverlayUI { get; set; }
            public IProgressionUI ProgressionUI { get; set; }
            public IWheelService Wheel { get; set; }
            public IZoneService Zone { get; set; }
            public IInventoryService Inventory { get; set; }
            public IRewardFlowService RewardFlow { get; set; }
            public IRewardResolver RewardResolver { get; set; }

            public void ChangeState(IGameState newState) { }
        }

        private sealed class FakeOverlayUI : IOverlayUI
        {
            public bool BombTensionCalled { get; private set; }

            public void ShowRevivePanel(bool isActive) { }

            public void PlayBombTensionAnimation(System.Action onComplete)
            {
                BombTensionCalled = true;
                onComplete?.Invoke();
            }
        }

        private sealed class FakeRewardFlowService : IRewardFlowService
        {
            public bool TransitionToInitCalled { get; private set; }
            public bool TransitionToGameOverCalled { get; private set; }

            public void TransitionToInitState()
            {
                TransitionToInitCalled = true;
            }

            public void TransitionToGameOverState()
            {
                TransitionToGameOverCalled = true;
            }
        }
    }
}