using System.Collections.Generic;
using UnityEngine;
using WheelGame.Contracts.Rewards;
using WheelGame.UI.Components;

namespace WheelGame.Gameplay.Wheel.Controllers
{
    public class WheelLayoutController
    {
        private readonly RectTransform _wheelContainer;
        private readonly WheelSliceUI _slicePrefab;
        private readonly float _sliceAngleOffset;
        private readonly float[] _perSliceAngleOffsets;

        public WheelLayoutController(RectTransform wheelContainer, WheelSliceUI slicePrefab, float sliceAngleOffset, float[] perSliceAngleOffsets)
        {
            _wheelContainer = wheelContainer;
            _slicePrefab = slicePrefab;
            _sliceAngleOffset = sliceAngleOffset;
            _perSliceAngleOffsets = perSliceAngleOffsets;
        }

        public int BuildSlices(List<IRewardAction> rewards)
        {
            int numberOfSlices = rewards.Count;

            foreach (Transform child in _wheelContainer)
            {
                Object.Destroy(child.gameObject);
            }

            float sliceAngle = 360f / numberOfSlices;

            for (int i = 0; i < numberOfSlices; i++)
            {
                WheelSliceUI newSlice = Object.Instantiate(_slicePrefab, _wheelContainer);
                newSlice.Configure(rewards[i]);
                float perSliceOffset = GetPerSliceAngleOffset(i);
                float visualAngle = (i * sliceAngle) + _sliceAngleOffset + perSliceOffset;
                newSlice.transform.localEulerAngles = new Vector3(0, 0, -visualAngle);
            }

            return numberOfSlices;
        }

        private float GetPerSliceAngleOffset(int sliceIndex)
        {
            if (_perSliceAngleOffsets == null || sliceIndex < 0 || sliceIndex >= _perSliceAngleOffsets.Length)
            {
                return 0f;
            }

            return _perSliceAngleOffsets[sliceIndex];
        }
    }
}