using UnityEngine;
using WheelGame.Contracts.StateMachine;
using WheelGame.Gameplay.StateMachine.States;

namespace WheelGame.Gameplay.Management
{
    public class GameCommandCoordinator
    {
        private readonly IGameContext _context;
        private readonly GameStateMachine _stateMachine;

        public GameCommandCoordinator(IGameContext context, GameStateMachine stateMachine)
        {
            _context = context;
            _stateMachine = stateMachine;
        }

        public void HandleSpinRequested()
        {
            if (_stateMachine.CurrentState is IdleState)
            {
                _stateMachine.ChangeState(new SpinningState());
            }
        }

        public void HandleWalkAwayRequested()
        {
            if (_stateMachine.CurrentState is IdleState)
            {
                Debug.Log("[GameCommandCoordinator] Walk Away triggered!");
                _stateMachine.ChangeState(new ClaimState());
            }
        }

        public void HandleReviveRequested()
        {
            if (_stateMachine.CurrentState is GameOverState)
            {
                Debug.Log("[GameCommandCoordinator] Player revived! Bomb avoided.");
                _context.Zone.IncreaseLevel();
                _stateMachine.ChangeState(new InitState());
            }
        }

        public void HandleGiveUpRequested()
        {
            if (_stateMachine.CurrentState is GameOverState)
            {
                Debug.Log("[GameCommandCoordinator] Player gave up. All progress is lost.");
                _context.Inventory.ClearInventory();
                _context.InventoryUI.ClearInventoryUI();
                _context.Zone.ResetLevel();
                _stateMachine.ChangeState(new InitState());
            }
        }
    }
}