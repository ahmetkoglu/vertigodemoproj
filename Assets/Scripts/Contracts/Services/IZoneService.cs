using System.Collections.Generic;
using UnityEngine;
using WheelGame.Contracts.Rewards;

namespace WheelGame.Contracts.Services
{
    public interface IZoneService
    {
        int CurrentLevel { get; }
        bool IsSuperZone { get; }
        bool IsSafeZone { get; }
        Sprite CurrentWheelSprite { get; }
        Sprite CurrentIndicatorSprite { get; }

        void IncreaseLevel();
        void ResetLevel();
        List<IRewardAction> GenerateNewWheel();
        IRewardAction GetRewardAtIndex(int index);
        int GetNextSafeZoneLevel();
        int GetNextSuperZoneLevel();
    }
}