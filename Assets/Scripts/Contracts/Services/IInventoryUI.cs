using System;
using UnityEngine;

namespace WheelGame.Contracts.Services
{
    public interface IInventoryUI
    {
        RectTransform PrepareInventorySlot(string itemId, Sprite icon, int currentAmount);
        void AnimateSlotAmount(string itemId, int newAmount);
        void ClearInventoryUI();
        void PlayRewardFlightAnimation(Sprite rewardIcon, Vector3 startWorldPos, RectTransform targetSlot, Action onComplete);
    }
}