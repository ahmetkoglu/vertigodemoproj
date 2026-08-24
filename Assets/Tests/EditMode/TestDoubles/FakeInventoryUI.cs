using System;
using UnityEngine;
using WheelGame.Contracts.Services;

namespace WheelGame.Tests.EditMode.TestDoubles
{
    public class FakeInventoryUI : IInventoryUI
    {
        public bool ClearCalled { get; private set; }
        public bool PrepareCalled { get; private set; }
        public bool AnimateCalled { get; private set; }
        public bool FlightAnimationCalled { get; private set; }
        public string LastPreparedItemId { get; private set; }
        public string LastAnimatedItemId { get; private set; }
        public int LastAnimatedAmount { get; private set; }
        public RectTransform PreparedSlot { get; set; }

        public RectTransform PrepareInventorySlot(string itemId, Sprite icon, int currentAmount)
        {
            PrepareCalled = true;
            LastPreparedItemId = itemId;
            return PreparedSlot;
        }

        public void AnimateSlotAmount(string itemId, int newAmount)
        {
            AnimateCalled = true;
            LastAnimatedItemId = itemId;
            LastAnimatedAmount = newAmount;
        }

        public void ClearInventoryUI()
        {
            ClearCalled = true;
        }

        public void PlayRewardFlightAnimation(Sprite rewardIcon, Vector3 startWorldPos, RectTransform targetSlot, Action onComplete)
        {
            FlightAnimationCalled = true;
            onComplete?.Invoke();
        }
    }
}