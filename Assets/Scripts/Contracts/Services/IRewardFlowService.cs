namespace WheelGame.Contracts.Services
{
    public interface IRewardFlowService
    {
        void TransitionToInitState();
        void TransitionToGameOverState();
    }
}