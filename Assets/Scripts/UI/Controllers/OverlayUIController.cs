using DG.Tweening;
using UnityEngine;

namespace WheelGame.UI.Controllers
{
    public class OverlayUIController
    {
        private readonly GameObject _revivePanel;
        private readonly CanvasGroup _exitPopupCanvasGroup;
        private readonly RectTransform _exitPopupBox;

        public OverlayUIController(GameObject revivePanel, CanvasGroup exitPopupCanvasGroup, RectTransform exitPopupBox)
        {
            _revivePanel = revivePanel;
            _exitPopupCanvasGroup = exitPopupCanvasGroup;
            _exitPopupBox = exitPopupBox;
        }

        public void ShowRevivePanel(bool isActive)
        {
            if (_revivePanel == null) return;

            _revivePanel.SetActive(isActive);
            if (isActive)
            {
                _revivePanel.transform.localScale = Vector3.zero;
                _revivePanel.transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.OutBack);
            }
        }

        public void ShowExitPopup()
        {
            if (_exitPopupCanvasGroup == null || _exitPopupBox == null) return;

            _exitPopupCanvasGroup.gameObject.SetActive(true);
            _exitPopupCanvasGroup.alpha = 0f;
            _exitPopupBox.localScale = Vector3.zero;

            _exitPopupCanvasGroup.DOFade(1f, 0.2f);
            _exitPopupBox.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }

        public void HideExitPopup()
        {
            if (_exitPopupCanvasGroup == null || _exitPopupBox == null) return;

            _exitPopupBox.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                _exitPopupCanvasGroup.alpha = 0f;
                _exitPopupCanvasGroup.gameObject.SetActive(false);
            });
        }
    }
}