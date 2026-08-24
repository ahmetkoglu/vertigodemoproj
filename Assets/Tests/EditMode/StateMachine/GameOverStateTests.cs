using NUnit.Framework;
using WheelGame.Gameplay.StateMachine.States;
using WheelGame.Tests.EditMode.TestDoubles;

namespace WheelGame.Tests.EditMode.StateMachine
{
    public class GameOverStateTests
    {
        private GameOverState _state;
        private FakeGameContext _context;
        private FakeInputControlUI _inputUI;
        private FakeOverlayUI _overlayUI;

        [SetUp]
        public void SetUp()
        {
            _state = new GameOverState();
            _inputUI = new FakeInputControlUI();
            _overlayUI = new FakeOverlayUI();
            _context = new FakeGameContext
            {
                InputUI = _inputUI,
                OverlayUI = _overlayUI
            };
        }

        [Test]
        public void EnterState_ShouldDisableInputButtons()
        {
            _state.EnterState(_context);

            Assert.IsFalse(_inputUI.SpinButtonState);
            Assert.IsFalse(_inputUI.WalkAwayButtonState);
        }

        [Test]
        public void EnterState_ShouldShowRevivePanel()
        {
            _state.EnterState(_context);

            Assert.AreEqual(true, _overlayUI.RevivePanelState);
        }

        [Test]
        public void ExitState_ShouldHideRevivePanel()
        {
            _state.ExitState(_context);

            Assert.AreEqual(false, _overlayUI.RevivePanelState);
        }
    }
}