using System;
using System.Collections.Generic;
using System.Linq;
using WheelGame.Contracts.Rewards;
using WheelGame.Contracts.StateMachine;

namespace WheelGame.Gameplay.Rewards.Resolution
{
    public class RewardResolver : IRewardResolver
    {
        private readonly List<IRewardResolutionHandler> _handlers;

        public RewardResolver(IEnumerable<IRewardResolutionHandler> handlers)
        {
            _handlers = handlers.ToList();
        }

        public void Resolve(IRewardAction reward, IGameContext context)
        {
            if (reward == null)
            {
                throw new ArgumentNullException(nameof(reward));
            }

            IRewardResolutionHandler handler = _handlers.FirstOrDefault(x => x.CanHandle(reward));
            if (handler != null)
            {
                handler.Resolve(reward, context);
                return;
            }

            throw new InvalidOperationException($"No reward resolution handler registered for reward type '{reward.Type}'.");
        }
    }
}