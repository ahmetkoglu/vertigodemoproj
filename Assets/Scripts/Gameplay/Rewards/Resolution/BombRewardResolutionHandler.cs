using WheelGame.Contracts.Rewards;
using WheelGame.Contracts.StateMachine;

namespace WheelGame.Gameplay.Rewards.Resolution
{
    public class BombRewardResolutionHandler : IRewardResolutionHandler
    {
        public bool CanHandle(IRewardAction reward)
        {
            return reward != null && reward.Type == RewardType.Bomb;
        }

        public void Resolve(IRewardAction reward, IGameContext context)
        {
            context.OverlayUI.PlayBombTensionAnimation(() =>
            {
                context.RewardFlow.TransitionToGameOverState();
            });
        }
    }
}