using NUnit.Framework;
using UnityEngine;
using WheelGame.Gameplay.Inventory;

namespace WheelGame.Tests.EditMode.Gameplay
{
    public class InventoryManagerTests
    {
        private GameObject _gameObject;
        private InventoryManager _inventoryManager;

        [SetUp]
        public void SetUp()
        {
            _gameObject = new GameObject("InventoryManagerTests");
            _inventoryManager = _gameObject.AddComponent<InventoryManager>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_gameObject);
        }

        [Test]
        public void AddItem_ShouldCreateNewEntry_WhenItemDoesNotExist()
        {
            _inventoryManager.AddItem("coin", 5);

            bool found = _inventoryManager.TryGetItemAmount("coin", out int amount);

            Assert.IsTrue(found);
            Assert.AreEqual(5, amount);
        }

        [Test]
        public void AddItem_ShouldIncreaseExistingAmount_WhenItemAlreadyExists()
        {
            _inventoryManager.AddItem("coin", 5);
            _inventoryManager.AddItem("coin", 3);

            bool found = _inventoryManager.TryGetItemAmount("coin", out int amount);

            Assert.IsTrue(found);
            Assert.AreEqual(8, amount);
        }

        [Test]
        public void ClearInventory_ShouldRemoveAllItems()
        {
            _inventoryManager.AddItem("coin", 5);
            _inventoryManager.ClearInventory();

            bool found = _inventoryManager.TryGetItemAmount("coin", out _);

            Assert.IsFalse(found);
        }

        [Test]
        public void TryGetItemAmount_ShouldReturnFalse_WhenItemDoesNotExist()
        {
            bool found = _inventoryManager.TryGetItemAmount("missing", out int amount);

            Assert.IsFalse(found);
            Assert.AreEqual(0, amount);
        }

        [Test]
        public void TryGetItemAmount_ShouldReturnTrueAndCorrectValue_WhenItemExists()
        {
            _inventoryManager.AddItem("cash", 12);

            bool found = _inventoryManager.TryGetItemAmount("cash", out int amount);

            Assert.IsTrue(found);
            Assert.AreEqual(12, amount);
        }
    }
}