using NUnit.Framework;
using WheelGame.Gameplay.StateMachine.States;
using WheelGame.Tests.EditMode.TestDoubles;

namespace WheelGame.Tests.EditMode.StateMachine
{
    public class IdleStateTests
    {
        private IdleState _state;
        private FakeGameContext _context;
        private FakeInputControlUI _inputUI;
        private FakeZoneService _zoneService;

        [SetUp]
        public void SetUp()
        {
            _state = new IdleState();
            _inputUI = new FakeInputControlUI();
            _zoneService = new FakeZoneService();
            _context = new FakeGameContext
            {
                InputUI = _inputUI,
                Zone = _zoneService
            };
        }

        [Test]
        public void EnterState_ShouldEnableSpinButton()
        {
            _state.EnterState(_context);

            Assert.IsTrue(_inputUI.SpinButtonState);
        }

        [Test]
        public void EnterState_ShouldEnableWalkAway_WhenZoneIsSafe()
        {
            _zoneService.IsSafeZone = true;

            _state.EnterState(_context);

            Assert.IsTrue(_inputUI.WalkAwayButtonState);
        }

        [Test]
        public void EnterState_ShouldEnableWalkAway_WhenZoneIsSuper()
        {
            _zoneService.IsSuperZone = true;

            _state.EnterState(_context);

            Assert.IsTrue(_inputUI.WalkAwayButtonState);
        }

        [Test]
        public void EnterState_ShouldDisableWalkAway_WhenZoneIsNormal()
        {
            _state.EnterState(_context);

            Assert.IsFalse(_inputUI.WalkAwayButtonState);
        }

        [Test]
        public void ExitState_ShouldDisableSpinAndWalkAway()
        {
            _state.ExitState(_context);

            Assert.IsFalse(_inputUI.SpinButtonState);
            Assert.IsFalse(_inputUI.WalkAwayButtonState);
        }
    }
}