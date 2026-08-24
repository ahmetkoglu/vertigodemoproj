using System.Collections.Generic;
using UnityEngine;
using WheelGame.Contracts.Rewards;
using WheelGame.Contracts.Services;

namespace WheelGame.Tests.EditMode.TestDoubles
{
    public class FakeZoneService : IZoneService
    {
        public int CurrentLevel { get; set; } = 1;
        public bool IsSuperZone { get; set; }
        public bool IsSafeZone { get; set; }
        public Sprite CurrentWheelSprite { get; set; }
        public Sprite CurrentIndicatorSprite { get; set; }
        public int NextSafeZoneLevel { get; set; }
        public int NextSuperZoneLevel { get; set; }
        public List<IRewardAction> GeneratedRewards { get; set; } = new List<IRewardAction>();
        public IRewardAction RewardAtIndex { get; set; }
        public bool IncreaseLevelCalled { get; private set; }
        public bool ResetLevelCalled { get; private set; }

        public void IncreaseLevel()
        {
            IncreaseLevelCalled = true;
            CurrentLevel++;
        }

        public void ResetLevel()
        {
            ResetLevelCalled = true;
            CurrentLevel = 1;
        }

        public List<IRewardAction> GenerateNewWheel()
        {
            return GeneratedRewards;
        }

        public IRewardAction GetRewardAtIndex(int index)
        {
            return RewardAtIndex;
        }

        public int GetNextSafeZoneLevel()
        {
            return NextSafeZoneLevel;
        }

        public int GetNextSuperZoneLevel()
        {
            return NextSuperZoneLevel;
        }
    }
}