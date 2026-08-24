using System;
using DG.Tweening;
using UnityEngine;

namespace WheelGame.Gameplay.Wheel.Controllers
{
    public class WheelSpinController
    {
        private readonly RectTransform _wheelContainer;
        private readonly int _minSpins;
        private readonly float _spinDuration;

        public WheelSpinController(RectTransform wheelContainer, int minSpins, float spinDuration)
        {
            _wheelContainer = wheelContainer;
            _minSpins = minSpins;
            _spinDuration = spinDuration;
        }

        public void SpinWheel(int numberOfSlices, int resultIndex, Action<int> onComplete)
        {
            float sliceAngle = 360f / numberOfSlices;
            float offsetAngle = (numberOfSlices - resultIndex) * sliceAngle;
            float targetAngle = (_minSpins * 360f) + offsetAngle;

            _wheelContainer.localEulerAngles = Vector3.zero;

            _wheelContainer.DORotate(new Vector3(0, 0, -targetAngle), _spinDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.OutElastic, 1.5f, 1f)
                .OnComplete(() =>
                {
                    Debug.Log($"[WheelSpinController] Rotation completed. Landed Index Marker: {resultIndex}");
                    onComplete?.Invoke(resultIndex);
                });
        }
    }
}