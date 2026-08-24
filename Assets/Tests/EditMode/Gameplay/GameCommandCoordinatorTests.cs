using NUnit.Framework;
using UnityEngine;
using WheelGame.Gameplay.Management;
using WheelGame.Gameplay.StateMachine.States;
using WheelGame.Tests.EditMode.TestDoubles;

namespace WheelGame.Tests.EditMode.Gameplay
{
    public class GameCommandCoordinatorTests
    {
        private FakeGameContext _context;
        private FakeZoneService _zoneService;
        private FakeInventoryService _inventoryService;
        private FakeInventoryUI _inventoryUI;
        private FakeInputControlUI _inputUI;
        private FakeOverlayUI _overlayUI;
        private FakeProgressionUI _progressionUI;
        private FakeWheelService _wheelService;
        private GameStateMachine _stateMachine;
        private GameCommandCoordinator _coordinator;

        [SetUp]
        public void SetUp()
        {
            _zoneService = new FakeZoneService
            {
                CurrentLevel = 1,
                GeneratedRewards = new System.Collections.Generic.List<WheelGame.Contracts.Rewards.IRewardAction>(),
                CurrentWheelSprite = Sprite.Create(Texture2D.blackTexture, new Rect(0, 0, 1, 1), Vector2.zero),
                CurrentIndicatorSprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 1, 1), Vector2.zero),
                NextSafeZoneLevel = 5,
                NextSuperZoneLevel = 30
            };
            _inventoryService = new FakeInventoryService();
            _inventoryUI = new FakeInventoryUI();
            _inputUI = new FakeInputControlUI();
            _overlayUI = new FakeOverlayUI();
            _progressionUI = new FakeProgressionUI();
            _wheelService = new FakeWheelService();
            _context = new FakeGameContext
            {
                InputUI = _inputUI,
                Zone = _zoneService,
                Inventory = _inventoryService,
                InventoryUI = _inventoryUI,
                OverlayUI = _overlayUI,
                ProgressionUI = _progressionUI,
                Wheel = _wheelService
            };
            _stateMachine = new GameStateMachine(_context);
            _coordinator = new GameCommandCoordinator(_context, _stateMachine);
        }

        [Test]
        public void HandleSpinRequested_ShouldStartSpinning_WhenCurrentStateIsIdle()
        {
            _stateMachine.ChangeState(new IdleState());

            _coordinator.HandleSpinRequested();

            Assert.IsInstanceOf<SpinningState>(_stateMachine.CurrentState);
        }

        [Test]
        public void HandleWalkAwayRequested_ShouldClaim_WhenCurrentStateIsIdle()
        {
            _stateMachine.ChangeState(new IdleState());

            _coordinator.HandleWalkAwayRequested();

            Assert.IsInstanceOf<ClaimState>(_stateMachine.CurrentState);
        }

        [Test]
        public void HandleReviveRequested_ShouldAdvanceLevel_WhenCurrentStateIsGameOver()
        {
            _stateMachine.ChangeState(new GameOverState());

            _coordinator.HandleReviveRequested();

            Assert.IsTrue(_zoneService.IncreaseLevelCalled);
            Assert.IsInstanceOf<InitState>(_stateMachine.CurrentState);
        }

        [Test]
        public void HandleGiveUpRequested_ShouldClearProgress_WhenCurrentStateIsGameOver()
        {
            _stateMachine.ChangeState(new GameOverState());

            _coordinator.HandleGiveUpRequested();

            Assert.IsTrue(_inventoryService.ClearCalled);
            Assert.IsTrue(_inventoryUI.ClearCalled);
            Assert.IsTrue(_zoneService.ResetLevelCalled);
            Assert.IsInstanceOf<InitState>(_stateMachine.CurrentState);
        }
    }
}