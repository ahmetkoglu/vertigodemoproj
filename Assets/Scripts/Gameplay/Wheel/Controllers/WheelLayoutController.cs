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

        public WheelLayoutController(RectTransform wheelContainer, WheelSliceUI slicePrefab)
        {
            _wheelContainer = wheelContainer;
            _slicePrefab = slicePrefab;
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
                newSlice.transform.localEulerAngles = new Vector3(0, 0, -i * sliceAngle);
            }

            return numberOfSlices;
        }
    }
}