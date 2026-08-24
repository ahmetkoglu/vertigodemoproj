using System;
using System.Collections.Generic;
using UnityEngine;
using WheelGame.Contracts.Rewards;

namespace WheelGame.Contracts.Services
{
    public interface IWheelService
    {
        event Action<int> OnSpinComplete;

        void SpinWheel(int resultIndex);
        void SetupWheel(List<IRewardAction> rewards);
        Vector3 GetWinningSlicePosition();
        void UpdateWheelVisuals(Sprite newWheelSprite, Sprite newIndicatorSprite, bool isSafeZone, bool isSuperZone);
    }
}