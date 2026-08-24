using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using WheelGame.Gameplay.Inventory;
using WheelGame.Gameplay.Management;
using WheelGame.Gameplay.Progression;
using WheelGame.Gameplay.Wheel;
using WheelGame.UI;

namespace WheelGame.Tests.PlayMode.Scene
{
    public class SceneValidationPlayModeTests
    {
        #region Scene Setup

        [UnitySetUp]
        public IEnumerator LoadScene()
        {
            SceneManager.LoadScene("SampleScene");
            yield return null;
        }

        #endregion

        #region Scene Validation Tests

        [Test]
        public void SampleScene_ShouldContainCoreManagers()
        {
            Assert.IsNotNull(Object.FindObjectOfType<GameManager>(), "GameManager was not found in SampleScene.");
            Assert.IsNotNull(Object.FindObjectOfType<UIManager>(), "UIManager was not found in SampleScene.");
            Assert.IsNotNull(Object.FindObjectOfType<WheelManager>(), "WheelManager was not found in SampleScene.");
            Assert.IsNotNull(Object.FindObjectOfType<ZoneManager>(), "ZoneManager was not found in SampleScene.");
            Assert.IsNotNull(Object.FindObjectOfType<InventoryManager>(), "InventoryManager was not found in SampleScene.");
        }

        [Test]
        public void GameManager_ShouldHaveCriticalReferencesAssigned()
        {
            GameManager gameManager = Object.FindObjectOfType<GameManager>();
            Assert.IsNotNull(gameManager);

            AssertSerializedFieldAssigned(gameManager, "uiManager");
            AssertSerializedFieldAssigned(gameManager, "wheelManager");
            AssertSerializedFieldAssigned(gameManager, "zoneManager");
            AssertSerializedFieldAssigned(gameManager, "inventoryManager");
        }

        [Test]
        public void UIManager_ShouldHaveCriticalReferencesAssigned()
        {
            UIManager uiManager = Object.FindObjectOfType<UIManager>();
            Assert.IsNotNull(uiManager);

            AssertSerializedFieldAssigned(uiManager, "mainBackgroundPanel");
            AssertSerializedFieldAssigned(uiManager, "revivePanel");
            AssertSerializedFieldAssigned(uiManager, "exitPopupCanvasGroup");
            AssertSerializedFieldAssigned(uiManager, "exitPopupBox");
            AssertSerializedFieldAssigned(uiManager, "mainCanvas");
            AssertSerializedFieldAssigned(uiManager, "uiEffectManager");
            AssertSerializedFieldAssigned(uiManager, "inventoryContainer");
            AssertSerializedFieldAssigned(uiManager, "inventoryItemPrefab");
            AssertSerializedFieldAssigned(uiManager, "levelContainer");
            AssertSerializedFieldAssigned(uiManager, "levelSlotPrefab");
            AssertSerializedFieldAssigned(uiManager, "btn_spin");
            AssertSerializedFieldAssigned(uiManager, "btn_walk_away");
            AssertSerializedFieldAssigned(uiManager, "btn_stay");
            AssertSerializedFieldAssigned(uiManager, "btn_leave");
            AssertSerializedFieldAssigned(uiManager, "btn_revive");
            AssertSerializedFieldAssigned(uiManager, "btn_give_up");
        }

        [Test]
        public void WheelManager_ShouldHaveCriticalReferencesAssigned()
        {
            WheelManager wheelManager = Object.FindObjectOfType<WheelManager>();
            Assert.IsNotNull(wheelManager);

            AssertSerializedFieldAssigned(wheelManager, "wheelBaseImage");
            AssertSerializedFieldAssigned(wheelManager, "indicatorImage");
            AssertSerializedFieldAssigned(wheelManager, "wheelGlowImage");
            AssertSerializedFieldAssigned(wheelManager, "wheelContainer");
            AssertSerializedFieldAssigned(wheelManager, "slicePrefab");
        }

        [Test]
        public void ZoneManager_ShouldHaveCriticalReferencesAssigned()
        {
            ZoneManager zoneManager = Object.FindObjectOfType<ZoneManager>();
            Assert.IsNotNull(zoneManager);

            AssertSerializedFieldAssigned(zoneManager, "normalZoneData");
            AssertSerializedFieldAssigned(zoneManager, "safeZoneData");
            AssertSerializedFieldAssigned(zoneManager, "superZoneData");
        }

        #endregion

        #region Validation Helpers

        private static void AssertSerializedFieldAssigned(Object target, string fieldName)
        {
            FieldInfo field = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(field, $"Field '{fieldName}' was not found on {target.GetType().Name}.");

            object value = field.GetValue(target);
            Assert.IsNotNull(value, $"Field '{fieldName}' on {target.GetType().Name} is not assigned in the scene.");
        }

        #endregion
    }
}