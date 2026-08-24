using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using WheelGame.Contracts.Rewards;
using WheelGame.Contracts.Services;
using WheelGame.Contracts.StateMachine;
using WheelGame.Gameplay.Management;
using WheelGame.Gameplay.StateMachine.States;
using WheelGame.UI.Controllers;

namespace WheelGame.Tests.PlayMode.Gameplay
{
    public class UIToGameplayFlowPlayModeTests
    {
        #region Runtime Test Objects

        private GameObject _root;
        private Button _spinButton;
        private Button _walkAwayButton;
        private Button _stayButton;
        private Button _leaveButton;
        private Button _reviveButton;
        private Button _giveUpButton;

        private UIInputController _inputController;
        private GameStateMachine _stateMachine;
        private GameCommandCoordinator _coordinator;

        private FakeGameContext _context;
        private FakeZoneService _zoneService;
        private FakeInventoryService _inventoryService;
        private FakeInventoryUI _inventoryUI;

        private bool _showExitPopupCalled;
        private bool _hideExitPopupCalled;
        private bool? _lastRevivePanelState;

        #endregion

        #region Test Lifecycle

        [SetUp]
        public void SetUp()
        {
            _root = new GameObject("UIToGameplayFlowPlayModeTests");
            _spinButton = CreateButton("SpinButton");
            _walkAwayButton = CreateButton("WalkAwayButton");
            _stayButton = CreateButton("StayButton");
            _leaveButton = CreateButton("LeaveButton");
            _reviveButton = CreateButton("ReviveButton");
            _giveUpButton = CreateButton("GiveUpButton");

            _zoneService = new FakeZoneService();
            _inventoryService = new FakeInventoryService();
            _inventoryUI = new FakeInventoryUI();

            _context = new FakeGameContext
            {
                InputUI = new FakeInputControlUI(),
                InventoryUI = _inventoryUI,
                OverlayUI = new FakeOverlayUI(),
                ProgressionUI = new FakeProgressionUI(),
                Wheel = new FakeWheelService(),
                Zone = _zoneService,
                Inventory = _inventoryService,
                RewardFlow = new FakeRewardFlowService(),
                RewardResolver = new FakeRewardResolver()
            };

            _stateMachine = new GameStateMachine(_context);
            _coordinator = new GameCommandCoordinator(_context, _stateMachine);

            _inputController = new UIInputController(
                _spinButton,
                _walkAwayButton,
                _stayButton,
                _leaveButton,
                _reviveButton,
                _giveUpButton,
                _coordinator.HandleSpinRequested,
                _coordinator.HandleWalkAwayRequested,
                _coordinator.HandleReviveRequested,
                _coordinator.HandleGiveUpRequested,
                () => _showExitPopupCalled = true,
                () => _hideExitPopupCalled = true,
                isActive => _lastRevivePanelState = isActive);

            _inputController.Bind();
        }

        [TearDown]
        public void TearDown()
        {
            _inputController?.Unbind();

            if (_root != null)
            {
                Object.DestroyImmediate(_root);
            }
        }

        #endregion

        #region UI To Gameplay Flow Tests

        [Test]
        public void SpinButtonClick_ShouldTransitionFromIdleToSpinning()
        {
            _stateMachine.ChangeState(new IdleState());

            _spinButton.onClick.Invoke();

            Assert.IsInstanceOf<SpinningState>(_stateMachine.CurrentState);
        }

        [Test]
        public void WalkAwayFlow_ShouldOpenPopupThenTransitionToClaimOnConfirm()
        {
            _stateMachine.ChangeState(new IdleState());

            _walkAwayButton.onClick.Invoke();
            Assert.IsTrue(_showExitPopupCalled);
            Assert.IsInstanceOf<IdleState>(_stateMachine.CurrentState);

            _leaveButton.onClick.Invoke();

            Assert.IsTrue(_hideExitPopupCalled);
            Assert.IsInstanceOf<ClaimState>(_stateMachine.CurrentState);
        }

        [Test]
        public void ReviveButtonClick_ShouldAdvanceLevelAndReturnToInit_WhenCurrentStateIsGameOver()
        {
            _stateMachine.ChangeState(new GameOverState());

            _reviveButton.onClick.Invoke();

            Assert.AreEqual(false, _lastRevivePanelState);
            Assert.IsTrue(_zoneService.IncreaseLevelCalled);
            Assert.IsInstanceOf<InitState>(_stateMachine.CurrentState);
        }

        [Test]
        public void GiveUpButtonClick_ShouldClearRunAndReturnToInit_WhenCurrentStateIsGameOver()
        {
            _stateMachine.ChangeState(new GameOverState());

            _giveUpButton.onClick.Invoke();

            Assert.AreEqual(false, _lastRevivePanelState);
            Assert.IsTrue(_inventoryService.ClearCalled);
            Assert.IsTrue(_inventoryUI.ClearCalled);
            Assert.IsTrue(_zoneService.ResetLevelCalled);
            Assert.IsInstanceOf<InitState>(_stateMachine.CurrentState);
        }

        #endregion

        #region Helpers

        private Button CreateButton(string name)
        {
            GameObject buttonObject = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
            buttonObject.transform.SetParent(_root.transform, false);
            return buttonObject.GetComponent<Button>();
        }

        private sealed class FakeGameContext : IGameContext
        {
            public IInputControlUI InputUI { get; set; }
            public IInventoryUI InventoryUI { get; set; }
            public IOverlayUI OverlayUI { get; set; }
            public IProgressionUI ProgressionUI { get; set; }
            public IWheelService Wheel { get; set; }
            public IZoneService Zone { get; set; }
            public IInventoryService Inventory { get; set; }
            public IRewardFlowService RewardFlow { get; set; }
            public IRewardResolver RewardResolver { get; set; }
            public IGameState LastChangedState { get; private set; }

            public void ChangeState(IGameState newState)
            {
                LastChangedState = newState;
            }
        }

        private sealed class FakeInputControlUI : IInputControlUI
        {
            public void SetSpinButtonState(bool isActive) { }
            public void SetWalkAwayButtonState(bool isActive) { }
        }

        private sealed class FakeInventoryUI : IInventoryUI
        {
            public bool ClearCalled { get; private set; }
            public RectTransform PrepareInventorySlot(string itemId, Sprite icon, int currentAmount) => null;
            public void AnimateSlotAmount(string itemId, int newAmount) { }
            public void ClearInventoryUI() => ClearCalled = true;
            public void PlayRewardFlightAnimation(Sprite rewardIcon, Vector3 startWorldPos, RectTransform targetSlot, System.Action onComplete) => onComplete?.Invoke();
        }

        private sealed class FakeOverlayUI : IOverlayUI
        {
            public void ShowRevivePanel(bool isActive) { }
            public void PlayBombTensionAnimation(System.Action onComplete) => onComplete?.Invoke();
        }

        private sealed class FakeProgressionUI : IProgressionUI
        {
            public void UpdateUpcomingZones(int nextSafeLevel, int nextSuperLevel) { }
            public void InitProgressBar(int currentLevel) { }
            public void UpdateLevelProgress(int currentLevel) { }
        }

        private sealed class FakeWheelService : IWheelService
        {
            public event System.Action<int> OnSpinComplete;
            public void SpinWheel(int resultIndex) => OnSpinComplete?.Invoke(resultIndex);
            public void SetupWheel(System.Collections.Generic.List<IRewardAction> rewards) { }
            public Vector3 GetWinningSlicePosition() => Vector3.zero;
            public void UpdateWheelVisuals(Sprite newWheelSprite, Sprite newIndicatorSprite, bool isSafeZone, bool isSuperZone) { }
        }

        private sealed class FakeZoneService : IZoneService
        {
            public int CurrentLevel => 1;
            public bool IsSuperZone => false;
            public bool IsSafeZone => false;
            public Sprite CurrentWheelSprite => null;
            public Sprite CurrentIndicatorSprite => null;
            public bool IncreaseLevelCalled { get; private set; }
            public bool ResetLevelCalled { get; private set; }

            public void IncreaseLevel() => IncreaseLevelCalled = true;
            public void ResetLevel() => ResetLevelCalled = true;
            public System.Collections.Generic.List<IRewardAction> GenerateNewWheel() => new System.Collections.Generic.List<IRewardAction>();
            public IRewardAction GetRewardAtIndex(int index) => null;
            public int GetNextSafeZoneLevel() => 5;
            public int GetNextSuperZoneLevel() => 30;
        }

        private sealed class FakeInventoryService : IInventoryService
        {
            public bool ClearCalled { get; private set; }
            public void AddItem(string itemId, int amount) { }
            public void ClearInventory() => ClearCalled = true;
            public bool TryGetItemAmount(string itemId, out int amount)
            {
                amount = 0;
                return false;
            }
        }

        private sealed class FakeRewardFlowService : IRewardFlowService
        {
            public void TransitionToInitState() { }
            public void TransitionToGameOverState() { }
        }

        private sealed class FakeRewardResolver : IRewardResolver
        {
            public void Resolve(IRewardAction reward, IGameContext context) { }
        }

        #endregion
    }
}