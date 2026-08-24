using System;
using UnityEngine;
using WheelGame.UI.Effects;

namespace WheelGame.UI.Controllers
{
    public class RewardAnimationController
    {
        private readonly Canvas _mainCanvas;
        private readonly UnityEngine.UI.Image _mainBackgroundPanel;
        private readonly UIEffectManager _uiEffectManager;

        public RewardAnimationController(Canvas mainCanvas, UnityEngine.UI.Image mainBackgroundPanel, UIEffectManager uiEffectManager)
        {
            _mainCanvas = mainCanvas;
            _mainBackgroundPanel = mainBackgroundPanel;
            _uiEffectManager = uiEffectManager;
        }

        public void PlayRewardFlightAnimation(Sprite rewardIcon, Vector3 startWorldPos, RectTransform targetSlot, Action onComplete)
        {
            if (_uiEffectManager == null)
            {
                onComplete?.Invoke();
                return;
            }

            _uiEffectManager.PlayCollectAnimation(_mainCanvas, rewardIcon, startWorldPos, targetSlot, onComplete);
        }

        public void PlayBombTensionAnimation(Action onComplete)
        {
            if (_uiEffectManager == null)
            {
                onComplete?.Invoke();
                return;
            }

            _uiEffectManager.PlayBombTensionAnimation(_mainBackgroundPanel, onComplete);
        }
    }
}