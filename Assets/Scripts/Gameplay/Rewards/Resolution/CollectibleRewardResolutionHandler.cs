using DG.Tweening;
using UnityEngine;
using WheelGame.Contracts.Rewards;
using WheelGame.Contracts.StateMachine;

namespace WheelGame.Gameplay.Rewards.Resolution
{
    public class CollectibleRewardResolutionHandler : IRewardResolutionHandler
    {
        public bool CanHandle(IRewardAction reward)
        {
            return reward is ICollectibleRewardAction && reward.Type != RewardType.Bomb;
        }

        public void Resolve(IRewardAction reward, IGameContext context)
        {
            ICollectibleRewardAction collectible = reward as ICollectibleRewardAction;
            if (collectible == null) return;

            int oldAmount = 0;
            context.Inventory.TryGetItemAmount(collectible.ItemId, out oldAmount);

            context.Inventory.AddItem(collectible.ItemId, collectible.Amount);
            context.Inventory.TryGetItemAmount(collectible.ItemId, out int newAmount);

            RectTransform targetSlot = context.InventoryUI.PrepareInventorySlot(collectible.ItemId, collectible.Icon, oldAmount);
            Vector3 startWorldPos = context.Wheel.GetWinningSlicePosition();

            bool isTransitioned = false;

            context.InventoryUI.PlayRewardFlightAnimation(collectible.Icon, startWorldPos, targetSlot, () =>
            {
                if (isTransitioned) return;
                isTransitioned = true;

                context.InventoryUI.AnimateSlotAmount(collectible.ItemId, newAmount);
                context.Zone.IncreaseLevel();
                context.RewardFlow.TransitionToInitState();
            });

            DOVirtual.DelayedCall(2.0f, () =>
            {
                if (isTransitioned) return;
                isTransitioned = true;

                Debug.LogWarning($"[CollectibleRewardResolutionHandler] Flight animation timed out for {collectible.ItemId}. Force-completing progression loop via fail-safe.");

                context.InventoryUI.AnimateSlotAmount(collectible.ItemId, newAmount);
                context.Zone.IncreaseLevel();
                context.RewardFlow.TransitionToInitState();
            });
        }
    }
}