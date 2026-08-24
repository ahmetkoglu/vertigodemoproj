using System;
using System.Collections.Generic;
using UnityEngine;
using WheelGame.Contracts.Rewards;
using WheelGame.Contracts.Services;

namespace WheelGame.Tests.EditMode.TestDoubles
{
    public class FakeWheelService : IWheelService
    {
        public event Action<int> OnSpinComplete;

        public bool SpinWheelCalled { get; private set; }
        public bool SetupWheelCalled { get; private set; }
        public bool UpdateWheelVisualsCalled { get; private set; }
        public int LastSpinResultIndex { get; private set; }
        public List<IRewardAction> LastSetupRewards { get; private set; }
        public Vector3 WinningSlicePosition { get; set; }

        public void SpinWheel(int resultIndex)
        {
            SpinWheelCalled = true;
            LastSpinResultIndex = resultIndex;
        }

        public void SetupWheel(List<IRewardAction> rewards)
        {
            SetupWheelCalled = true;
            LastSetupRewards = rewards;
        }

        public Vector3 GetWinningSlicePosition()
        {
            return WinningSlicePosition;
        }

        public void UpdateWheelVisuals(Sprite newWheelSprite, Sprite newIndicatorSprite, bool isSafeZone, bool isSuperZone)
        {
            UpdateWheelVisualsCalled = true;
        }

        public void RaiseSpinComplete(int index)
        {
            OnSpinComplete?.Invoke(index);
        }
    }
}