using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using WheelGame.UI.Controllers;

namespace WheelGame.Tests.PlayMode.UI
{
    public class UIInputControllerPlayModeTests
    {
        private GameObject _root;
        private Button _spinButton;
        private Button _walkAwayButton;
        private Button _stayButton;
        private Button _leaveButton;
        private Button _reviveButton;
        private Button _giveUpButton;
        private UIInputController _controller;

        private bool _spinRequested;
        private bool _walkAwayRequested;
        private bool _reviveRequested;
        private bool _giveUpRequested;
        private bool _showExitPopupCalled;
        private bool _hideExitPopupCalled;
        private bool? _lastRevivePanelState;

        [SetUp]
        public void SetUp()
        {
            _root = new GameObject("UIInputControllerPlayModeTests");

            _spinButton = CreateButton("SpinButton");
            _walkAwayButton = CreateButton("WalkAwayButton");
            _stayButton = CreateButton("StayButton");
            _leaveButton = CreateButton("LeaveButton");
            _reviveButton = CreateButton("ReviveButton");
            _giveUpButton = CreateButton("GiveUpButton");

            _spinRequested = false;
            _walkAwayRequested = false;
            _reviveRequested = false;
            _giveUpRequested = false;
            _showExitPopupCalled = false;
            _hideExitPopupCalled = false;
            _lastRevivePanelState = null;

            _controller = new UIInputController(
                _spinButton,
                _walkAwayButton,
                _stayButton,
                _leaveButton,
                _reviveButton,
                _giveUpButton,
                () => _spinRequested = true,
                () => _walkAwayRequested = true,
                () => _reviveRequested = true,
                () => _giveUpRequested = true,
                () => _showExitPopupCalled = true,
                () => _hideExitPopupCalled = true,
                isActive => _lastRevivePanelState = isActive);

            _controller.Bind();
        }

        [TearDown]
        public void TearDown()
        {
            _controller?.Unbind();

            if (_root != null)
            {
                Object.DestroyImmediate(_root);
            }
        }

        [Test]
        public void SpinButtonClick_ShouldInvokeSpinRequestedCallback()
        {
            _spinButton.onClick.Invoke();

            Assert.IsTrue(_spinRequested);
        }

        [Test]
        public void WalkAwayButtonClick_ShouldOnlyRequestExitPopup()
        {
            _walkAwayButton.onClick.Invoke();

            Assert.IsTrue(_showExitPopupCalled);
            Assert.IsFalse(_walkAwayRequested);
        }

        [Test]
        public void StayButtonClick_ShouldHideExitPopup()
        {
            _stayButton.onClick.Invoke();

            Assert.IsTrue(_hideExitPopupCalled);
        }

        [Test]
        public void LeaveButtonClick_ShouldHideExitPopupAndInvokeWalkAwayRequested()
        {
            _leaveButton.onClick.Invoke();

            Assert.IsTrue(_hideExitPopupCalled);
            Assert.IsTrue(_walkAwayRequested);
        }

        [Test]
        public void ReviveButtonClick_ShouldHideRevivePanelAndInvokeReviveRequested()
        {
            _reviveButton.onClick.Invoke();

            Assert.AreEqual(false, _lastRevivePanelState);
            Assert.IsTrue(_reviveRequested);
        }

        [Test]
        public void GiveUpButtonClick_ShouldHideRevivePanelAndInvokeGiveUpRequested()
        {
            _giveUpButton.onClick.Invoke();

            Assert.AreEqual(false, _lastRevivePanelState);
            Assert.IsTrue(_giveUpRequested);
        }

        private Button CreateButton(string name)
        {
            GameObject buttonObject = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
            buttonObject.transform.SetParent(_root.transform, false);
            return buttonObject.GetComponent<Button>();
        }
    }
}