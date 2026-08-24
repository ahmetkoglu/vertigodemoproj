using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace WheelGame.Gameplay.Wheel.Controllers
{
    public class WheelAuraController
    {
        private readonly Image _wheelGlowImage;

        public WheelAuraController(Image wheelGlowImage)
        {
            _wheelGlowImage = wheelGlowImage;
        }

        public void UpdateAura(bool isSafeZone, bool isSuperZone)
        {
            if (_wheelGlowImage == null) return;

            _wheelGlowImage.DOKill();
            _wheelGlowImage.transform.DOKill();

            if (isSafeZone || isSuperZone)
            {
                if (isSuperZone)
                {
                    _wheelGlowImage.color = new Color(1f, 0.8f, 0f, 0f);
                }
                else if (isSafeZone)
                {
                    _wheelGlowImage.color = new Color(0.85f, 0.85f, 0.9f, 0f);
                }

                Sequence auraSeq = DOTween.Sequence();
                auraSeq.Append(_wheelGlowImage.DOFade(0.6f, 1f));
                auraSeq.Join(_wheelGlowImage.transform.DOScale(1.1f, 1.5f)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.InOutSine));
            }
            else
            {
                _wheelGlowImage.DOFade(0f, 0.5f);
                _wheelGlowImage.transform.localScale = Vector3.one;
            }
        }
    }
}