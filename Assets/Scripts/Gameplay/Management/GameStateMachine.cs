using UnityEngine;
using WheelGame.Contracts.StateMachine;

namespace WheelGame.Gameplay.Management
{
    public class GameStateMachine
    {
        private readonly IGameContext _context;

        private IGameState _currentState;

        public IGameState CurrentState => _currentState;

        public GameStateMachine(IGameContext context)
        {
            _context = context;
        }

        public void ChangeState(IGameState newState)
        {
            if (_currentState != null)
            {
                _currentState.ExitState(_context);
                Debug.Log($"[GameStateMachine] Exited State: {_currentState.GetType().Name}");
            }

            _currentState = newState;

            Debug.Log($"[GameStateMachine] Entered State: {_currentState.GetType().Name}");
            _currentState.EnterState(_context);
        }
    }
}