using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace WheelGame.Gameplay.Wheel.Controllers
{
    public class WheelVisualController
    {
        private readonly Image _wheelBaseImage;
        private readonly Image _indicatorImage;
        private readonly WheelAuraController _wheelAuraController;

        public WheelVisualController(Image wheelBaseImage, Image indicatorImage, WheelAuraController wheelAuraController)
        {
            _wheelBaseImage = wheelBaseImage;
            _indicatorImage = indicatorImage;
            _wheelAuraController = wheelAuraController;
        }

        public void UpdateWheelVisuals(Sprite newWheelSprite, Sprite newIndicatorSprite, bool isSafeZone, bool isSuperZone)
        {
            if (_wheelBaseImage.sprite == newWheelSprite)
            {
                _wheelBaseImage.transform.DOPunchScale(Vector3.one * 0.05f, 0.3f, 10, 1f);
                return;
            }

            _wheelBaseImage.transform.DOKill();
            if (_indicatorImage != null) _indicatorImage.transform.DOKill();

            Sequence seq = DOTween.Sequence();
            seq.Append(_wheelBaseImage.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack));
            if (_indicatorImage != null)
            {
                seq.Join(_indicatorImage.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack));
            }

            seq.AppendCallback(() =>
            {
                _wheelBaseImage.sprite = newWheelSprite;
                if (_indicatorImage != null) _indicatorImage.sprite = newIndicatorSprite;
            });

            seq.Append(_wheelBaseImage.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack));
            if (_indicatorImage != null)
            {
                seq.Join(_indicatorImage.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack));
            }

            seq.Join(_wheelBaseImage.transform.DORotate(new Vector3(0, 0, -360), 0.5f, RotateMode.FastBeyond360).SetRelative());

            _wheelAuraController?.UpdateAura(isSafeZone, isSuperZone);
        }
    }
}