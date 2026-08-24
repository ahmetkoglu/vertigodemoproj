using System;
using UnityEngine.UI;

namespace WheelGame.UI.Controllers
{
    public class UIInputController
    {
        private readonly Button _spinButton;
        private readonly Button _walkAwayButton;
        private readonly Button _stayButton;
        private readonly Button _leaveButton;
        private readonly Button _reviveButton;
        private readonly Button _giveUpButton;

        private readonly Action _onSpinRequested;
        private readonly Action _onWalkAwayRequested;
        private readonly Action _onReviveRequested;
        private readonly Action _onGiveUpRequested;
        private readonly Action _showExitPopup;
        private readonly Action _hideExitPopup;
        private readonly Action<bool> _showRevivePanel;

        public UIInputController(
            Button spinButton,
            Button walkAwayButton,
            Button stayButton,
            Button leaveButton,
            Button reviveButton,
            Button giveUpButton,
            Action onSpinRequested,
            Action onWalkAwayRequested,
            Action onReviveRequested,
            Action onGiveUpRequested,
            Action showExitPopup,
            Action hideExitPopup,
            Action<bool> showRevivePanel)
        {
            _spinButton = spinButton;
            _walkAwayButton = walkAwayButton;
            _stayButton = stayButton;
            _leaveButton = leaveButton;
            _reviveButton = reviveButton;
            _giveUpButton = giveUpButton;
            _onSpinRequested = onSpinRequested;
            _onWalkAwayRequested = onWalkAwayRequested;
            _onReviveRequested = onReviveRequested;
            _onGiveUpRequested = onGiveUpRequested;
            _showExitPopup = showExitPopup;
            _hideExitPopup = hideExitPopup;
            _showRevivePanel = showRevivePanel;
        }

        public void Bind()
        {
            if (_spinButton != null) _spinButton.onClick.AddListener(HandleSpinButtonClicked);
            if (_walkAwayButton != null) _walkAwayButton.onClick.AddListener(HandleWalkAwayButtonClicked);
            if (_stayButton != null) _stayButton.onClick.AddListener(HandleStayButtonClicked);
            if (_leaveButton != null) _leaveButton.onClick.AddListener(HandleWalkAwayConfirmed);
            if (_reviveButton != null) _reviveButton.onClick.AddListener(HandleReviveButtonClicked);
            if (_giveUpButton != null) _giveUpButton.onClick.AddListener(HandleGiveUpButtonClicked);
        }

        public void Unbind()
        {
            if (_spinButton != null) _spinButton.onClick.RemoveListener(HandleSpinButtonClicked);
            if (_walkAwayButton != null) _walkAwayButton.onClick.RemoveListener(HandleWalkAwayButtonClicked);
            if (_stayButton != null) _stayButton.onClick.RemoveListener(HandleStayButtonClicked);
            if (_leaveButton != null) _leaveButton.onClick.RemoveListener(HandleWalkAwayConfirmed);
            if (_reviveButton != null) _reviveButton.onClick.RemoveListener(HandleReviveButtonClicked);
            if (_giveUpButton != null) _giveUpButton.onClick.RemoveListener(HandleGiveUpButtonClicked);
        }

        public void SetSpinButtonState(bool isActive)
        {
            if (_spinButton != null) _spinButton.interactable = isActive;
        }

        public void SetWalkAwayButtonState(bool isActive)
        {
            if (_walkAwayButton != null) _walkAwayButton.interactable = isActive;
        }

        private void HandleSpinButtonClicked()
        {
            _onSpinRequested?.Invoke();
        }

        private void HandleWalkAwayButtonClicked()
        {
            _showExitPopup?.Invoke();
        }

        private void HandleStayButtonClicked()
        {
            _hideExitPopup?.Invoke();
        }

        private void HandleWalkAwayConfirmed()
        {
            _hideExitPopup?.Invoke();
            _onWalkAwayRequested?.Invoke();
        }

        private void HandleReviveButtonClicked()
        {
            _showRevivePanel?.Invoke(false);
            _onReviveRequested?.Invoke();
        }

        private void HandleGiveUpButtonClicked()
        {
            _showRevivePanel?.Invoke(false);
            _onGiveUpRequested?.Invoke();
        }
    }
}