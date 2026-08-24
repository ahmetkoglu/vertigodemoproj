using WheelGame.Contracts.Services;
using WheelGame.Gameplay.StateMachine.States;

namespace WheelGame.Gameplay.Management
{
    public class GameRewardFlowController : IRewardFlowService
    {
        private readonly GameStateMachine _stateMachine;

        public GameRewardFlowController(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void TransitionToInitState()
        {
            _stateMachine.ChangeState(new InitState());
        }

        public void TransitionToGameOverState()
        {
            _stateMachine.ChangeState(new GameOverState());
        }
    }
}