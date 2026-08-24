using NUnit.Framework;
using WheelGame.Contracts.StateMachine;
using WheelGame.Gameplay.Management;
using WheelGame.Tests.EditMode.TestDoubles;

namespace WheelGame.Tests.EditMode.Gameplay
{
    public class GameStateMachineTests
    {
        private FakeGameContext _context;
        private GameStateMachine _stateMachine;

        [SetUp]
        public void SetUp()
        {
            _context = new FakeGameContext();
            _stateMachine = new GameStateMachine(_context);
        }

        [Test]
        public void ChangeState_ShouldEnterNewState()
        {
            SpyState state = new SpyState();

            _stateMachine.ChangeState(state);

            Assert.IsTrue(state.EnterCalled);
            Assert.AreSame(_context, state.LastContext);
        }

        [Test]
        public void ChangeState_ShouldExitPreviousState()
        {
            SpyState previousState = new SpyState();
            SpyState nextState = new SpyState();

            _stateMachine.ChangeState(previousState);
            _stateMachine.ChangeState(nextState);

            Assert.IsTrue(previousState.ExitCalled);
            Assert.AreSame(_context, previousState.LastContext);
        }

        [Test]
        public void ChangeState_ShouldReplaceCurrentState()
        {
            SpyState previousState = new SpyState();
            SpyState nextState = new SpyState();

            _stateMachine.ChangeState(previousState);
            _stateMachine.ChangeState(nextState);

            Assert.AreSame(nextState, _stateMachine.CurrentState);
        }

        private sealed class SpyState : IGameState
        {
            public bool EnterCalled { get; private set; }
            public bool ExitCalled { get; private set; }
            public IGameContext LastContext { get; private set; }

            public void EnterState(IGameContext context)
            {
                EnterCalled = true;
                LastContext = context;
            }

            public void ExitState(IGameContext context)
            {
                ExitCalled = true;
                LastContext = context;
            }
        }
    }
}