using UnityEngine;

namespace WheelGame.Gameplay.Wheel.Controllers
{
    public class WheelPositionResolver
    {
        private readonly RectTransform _wheelContainer;
        private readonly float _topOffset;

        public WheelPositionResolver(RectTransform wheelContainer, float topOffset = 300f)
        {
            _wheelContainer = wheelContainer;
            _topOffset = topOffset;
        }

        public Vector3 GetWinningSlicePosition()
        {
            return _wheelContainer.position + new Vector3(0, _topOffset, 0);
        }
    }
}