using NUnit.Framework;
using WheelGame.Gameplay.StateMachine.States;
using WheelGame.Tests.EditMode.TestDoubles;

namespace WheelGame.Tests.EditMode.StateMachine
{
    public class ClaimStateTests
    {
        private ClaimState _state;
        private FakeGameContext _context;
        private FakeInventoryService _inventoryService;
        private FakeInventoryUI _inventoryUI;
        private FakeZoneService _zoneService;

        [SetUp]
        public void SetUp()
        {
            _state = new ClaimState();
            _inventoryService = new FakeInventoryService();
            _inventoryUI = new FakeInventoryUI();
            _zoneService = new FakeZoneService();
            _context = new FakeGameContext
            {
                Inventory = _inventoryService,
                InventoryUI = _inventoryUI,
                Zone = _zoneService
            };
        }

        [Test]
        public void EnterState_ShouldClearInventory()
        {
            _state.EnterState(_context);

            Assert.IsTrue(_inventoryService.ClearCalled);
        }

        [Test]
        public void EnterState_ShouldClearInventoryUI()
        {
            _state.EnterState(_context);

            Assert.IsTrue(_inventoryUI.ClearCalled);
        }

        [Test]
        public void EnterState_ShouldResetZone()
        {
            _state.EnterState(_context);

            Assert.IsTrue(_zoneService.ResetLevelCalled);
        }

        [Test]
        public void EnterState_ShouldTransitionToInitState()
        {
            _state.EnterState(_context);

            Assert.IsInstanceOf<InitState>(_context.LastChangedState);
        }
    }
}