using NUnit.Framework;
using UnityEngine;
using WheelGame.Contracts.Rewards;
using WheelGame.Gameplay.StateMachine.States;
using WheelGame.Tests.EditMode.TestDoubles;

namespace WheelGame.Tests.EditMode.StateMachine
{
    public class InitStateTests
    {
        private InitState _state;
        private FakeGameContext _context;
        private FakeProgressionUI _progressionUI;
        private FakeZoneService _zoneService;
        private FakeWheelService _wheelService;

        [SetUp]
        public void SetUp()
        {
            _state = new InitState();
            _progressionUI = new FakeProgressionUI();
            _zoneService = new FakeZoneService
            {
                CurrentLevel = 1,
                GeneratedRewards = new System.Collections.Generic.List<IRewardAction>(),
                CurrentWheelSprite = Sprite.Create(Texture2D.blackTexture, new Rect(0, 0, 1, 1), Vector2.zero),
                CurrentIndicatorSprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 1, 1), Vector2.zero),
                NextSafeZoneLevel = 5,
                NextSuperZoneLevel = 30
            };
            _wheelService = new FakeWheelService();
            _context = new FakeGameContext
            {
                ProgressionUI = _progressionUI,
                Zone = _zoneService,
                Wheel = _wheelService
            };
        }

        [Test]
        public void EnterState_ShouldUpdateWheelVisuals()
        {
            _state.EnterState(_context);

            Assert.IsTrue(_wheelService.UpdateWheelVisualsCalled);
        }

        [Test]
        public void EnterState_ShouldGenerateAndSetupWheel()
        {
            _state.EnterState(_context);

            Assert.IsTrue(_wheelService.SetupWheelCalled);
            Assert.AreSame(_zoneService.GeneratedRewards, _wheelService.LastSetupRewards);
        }

        [Test]
        public void EnterState_ShouldUpdateUpcomingZones()
        {
            _state.EnterState(_context);

            Assert.IsTrue(_progressionUI.UpdateUpcomingZonesCalled);
            Assert.AreEqual(5, _progressionUI.LastSafeZoneLevel);
            Assert.AreEqual(30, _progressionUI.LastSuperZoneLevel);
        }

        [Test]
        public void EnterState_ShouldInitializeProgressBar_WhenLevelIsOne()
        {
            _zoneService.CurrentLevel = 1;

            _state.EnterState(_context);

            Assert.IsTrue(_progressionUI.InitProgressBarCalled);
            Assert.IsTrue(_progressionUI.UpdateProgressCalled);
            Assert.AreEqual(1, _progressionUI.LastProgressLevel);
        }

        [Test]
        public void EnterState_ShouldUpdateProgress_WhenLevelIsGreaterThanOne()
        {
            _zoneService.CurrentLevel = 7;

            _state.EnterState(_context);

            Assert.IsFalse(_progressionUI.InitProgressBarCalled);
            Assert.IsTrue(_progressionUI.UpdateProgressCalled);
            Assert.AreEqual(7, _progressionUI.LastProgressLevel);
        }

        [Test]
        public void EnterState_ShouldTransitionToIdleState()
        {
            _state.EnterState(_context);

            Assert.IsInstanceOf<IdleState>(_context.LastChangedState);
        }
    }
}