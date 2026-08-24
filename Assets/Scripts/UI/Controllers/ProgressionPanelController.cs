using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WheelGame.UI.Components;

namespace WheelGame.UI.Controllers
{
    public class ProgressionPanelController
    {
        private readonly TextMeshProUGUI _nextSafeZoneText;
        private readonly TextMeshProUGUI _nextSuperZoneText;
        private readonly RectTransform _levelContainer;
        private readonly LevelSlotUI _levelSlotPrefab;
        private readonly float _slotWidth;
        private readonly int _visibleFutureLevels;

        public ProgressionPanelController(
            TextMeshProUGUI nextSafeZoneText,
            TextMeshProUGUI nextSuperZoneText,
            RectTransform levelContainer,
            LevelSlotUI levelSlotPrefab,
            float slotWidth,
            int visibleFutureLevels)
        {
            _nextSafeZoneText = nextSafeZoneText;
            _nextSuperZoneText = nextSuperZoneText;
            _levelContainer = levelContainer;
            _levelSlotPrefab = levelSlotPrefab;
            _slotWidth = slotWidth;
            _visibleFutureLevels = visibleFutureLevels;
        }

        public void InitProgressBar(int currentLevel)
        {
            for (int i = 0; i < _visibleFutureLevels; i++)
            {
                CreateLevelSlot(currentLevel + i);
            }
        }

        public void UpdateLevelProgress(int currentLevel)
        {
            if (_levelContainer == null) return;

            LayoutRebuilder.ForceRebuildLayoutImmediate(_levelContainer);
            float targetX = -(currentLevel - 1) * _slotWidth;

            _levelContainer.DOKill();
            _levelContainer.DOAnchorPosX(targetX, 0.6f).SetEase(Ease.OutBack);
        }

        public void UpdateUpcomingZones(int nextSafeLevel, int nextSuperLevel)
        {
            if (_nextSafeZoneText != null) _nextSafeZoneText.text = $"Safe Zone: {nextSafeLevel}";
            if (_nextSuperZoneText != null) _nextSuperZoneText.text = $"Super Zone: {nextSuperLevel}";
        }

        private void CreateLevelSlot(int level)
        {
            LevelSlotUI newSlot = UnityEngine.Object.Instantiate(_levelSlotPrefab, _levelContainer);
            bool isSuper = level % 30 == 0;
            bool isSafe = level % 5 == 0 && !isSuper;
            newSlot.Configure(level, isSafe, isSuper);
        }
    }
}