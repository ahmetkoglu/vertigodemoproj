using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using WheelGame.Contracts.Rewards;
using WheelGame.Contracts.Services;
using WheelGame.Contracts.StateMachine;
using WheelGame.Gameplay.Rewards;
using WheelGame.Gameplay.Rewards.Resolution;
using WheelGame.Gameplay.StateMachine.States;

namespace WheelGame.Tests.PlayMode.Rewards
{
    public class CollectibleRewardResolutionHandlerPlayModeTests
    {
        private RectTransform _preparedSlot;

        [SetUp]
        public void SetUp()
        {
            GameObject slotObject = new GameObject("PreparedSlot", typeof(RectTransform));
            _preparedSlot = slotObject.GetComponent<RectTransform>();
        }

        [TearDown]
        public void TearDown()
        {
            if (_preparedSlot != null)
            {
                UnityEngine.Object.DestroyImmediate(_preparedSlot.gameObject);
            }
        }

        [UnityTest]
        public IEnumerator Resolve_ShouldApplyRewardUpdateUiAdvanceLevelAndTransitionToInit()
        {
            CollectibleReward reward = ScriptableObject.CreateInstance<CollectibleReward>();
            reward.itemId = "coin";
            reward.amount = 5;
            reward.icon = Sprite.Create(Texture2D.blackTexture, new Rect(0, 0, 1, 1), Vector2.zero);

            FakeInventoryService inventory = new FakeInventoryService();
            FakeInventoryUI inventoryUi = new FakeInventoryUI(_preparedSlot);
            FakeWheelService wheel = new FakeWheelService();
            FakeZoneService zone = new FakeZoneService();
            FakeRewardFlowService rewardFlow = new FakeRewardFlowService();

            FakeGameContext context = new FakeGameContext
            {
                Inventory = inventory,
                InventoryUI = inventoryUi,
                Wheel = wheel,
                Zone = zone,
                RewardFlow = rewardFlow
            };

            CollectibleRewardResolutionHandler handler = new CollectibleRewardResolutionHandler();

            handler.Resolve(reward, context);

            yield return null;

            Assert.AreEqual(5, inventory.GetAmount("coin"));
            Assert.IsTrue(inventoryUi.PrepareCalled);
            Assert.IsTrue(inventoryUi.FlightAnimationCalled);
            Assert.IsTrue(inventoryUi.AnimateCalled);
            Assert.IsTrue(zone.IncreaseLevelCalled);
            Assert.IsTrue(rewardFlow.TransitionToInitCalled);
            Assert.IsFalse(rewardFlow.TransitionToGameOverCalled);

            UnityEngine.Object.DestroyImmediate(reward);
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

        private sealed class FakeInventoryService : IInventoryService
        {
            private readonly System.Collections.Generic.Dictionary<string, int> _items = new System.Collections.Generic.Dictionary<string, int>();

            public void AddItem(string itemId, int amount)
            {
                _items[itemId] = GetAmount(itemId) + amount;
            }

            public void ClearInventory()
            {
                _items.Clear();
            }

            public bool TryGetItemAmount(string itemId, out int amount)
            {
                return _items.TryGetValue(itemId, out amount);
            }

            public int GetAmount(string itemId)
            {
                return _items.TryGetValue(itemId, out int amount) ? amount : 0;
            }
        }

        private sealed class FakeInventoryUI : IInventoryUI
        {
            private readonly RectTransform _preparedSlot;

            public FakeInventoryUI(RectTransform preparedSlot)
            {
                _preparedSlot = preparedSlot;
            }

            public bool PrepareCalled { get; private set; }
            public bool AnimateCalled { get; private set; }
            public bool FlightAnimationCalled { get; private set; }

            public RectTransform PrepareInventorySlot(string itemId, Sprite icon, int currentAmount)
            {
                PrepareCalled = true;
                return _preparedSlot;
            }

            public void AnimateSlotAmount(string itemId, int newAmount)
            {
                AnimateCalled = true;
            }

            public void ClearInventoryUI()
            {
            }

            public void PlayRewardFlightAnimation(Sprite rewardIcon, Vector3 startWorldPos, RectTransform targetSlot, Action onComplete)
            {
                FlightAnimationCalled = true;
                onComplete?.Invoke();
            }
        }

        private sealed class FakeWheelService : IWheelService
        {
            public event Action<int> OnSpinComplete;

            public void SpinWheel(int resultIndex)
            {
                OnSpinComplete?.Invoke(resultIndex);
            }

            public void SetupWheel(System.Collections.Generic.List<IRewardAction> rewards)
            {
            }

            public Vector3 GetWinningSlicePosition()
            {
                return Vector3.zero;
            }

            public void UpdateWheelVisuals(Sprite newWheelSprite, Sprite newIndicatorSprite, bool isSafeZone, bool isSuperZone)
            {
            }
        }

        private sealed class FakeZoneService : IZoneService
        {
            public int CurrentLevel => 1;
            public bool IsSuperZone => false;
            public bool IsSafeZone => false;
            public Sprite CurrentWheelSprite => null;
            public Sprite CurrentIndicatorSprite => null;
            public bool IncreaseLevelCalled { get; private set; }

            public void IncreaseLevel()
            {
                IncreaseLevelCalled = true;
            }

            public void ResetLevel()
            {
            }

            public System.Collections.Generic.List<IRewardAction> GenerateNewWheel()
            {
                return new System.Collections.Generic.List<IRewardAction>();
            }

            public IRewardAction GetRewardAtIndex(int index)
            {
                return null;
            }

            public int GetNextSafeZoneLevel()
            {
                return 5;
            }

            public int GetNextSuperZoneLevel()
            {
                return 30;
            }
        }

        private sealed class FakeRewardFlowService : IRewardFlowService
        {
            public bool TransitionToInitCalled { get; private set; }
            public bool TransitionToGameOverCalled { get; private set; }

            public void TransitionToInitState()
            {
                TransitionToInitCalled = true;
            }

            public void TransitionToGameOverState()
            {
                TransitionToGameOverCalled = true;
            }
        }
    }
}