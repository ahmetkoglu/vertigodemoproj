using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WheelGame.UI.Components;

namespace WheelGame.UI.Controllers
{
    public class InventoryPanelController
    {
        private readonly Transform _inventoryContainer;
        private readonly InventoryItemUI _inventoryItemPrefab;
        private readonly Dictionary<string, InventoryItemUI> _activeUISlots = new Dictionary<string, InventoryItemUI>();

        public InventoryPanelController(Transform inventoryContainer, InventoryItemUI inventoryItemPrefab)
        {
            _inventoryContainer = inventoryContainer;
            _inventoryItemPrefab = inventoryItemPrefab;
        }

        public RectTransform PrepareInventorySlot(string itemId, Sprite icon, int currentAmount)
        {
            if (_activeUISlots.ContainsKey(itemId))
            {
                return _activeUISlots[itemId].GetComponent<RectTransform>();
            }

            InventoryItemUI newSlot = UnityEngine.Object.Instantiate(_inventoryItemPrefab, _inventoryContainer);
            newSlot.InitSlot(icon, currentAmount);
            newSlot.Appear();

            _activeUISlots.Add(itemId, newSlot);
            LayoutRebuilder.ForceRebuildLayoutImmediate(_inventoryContainer as RectTransform);

            return newSlot.GetComponent<RectTransform>();
        }

        public void AnimateSlotAmount(string itemId, int newAmount)
        {
            if (_activeUISlots.ContainsKey(itemId))
            {
                _activeUISlots[itemId].AnimateAmount(newAmount);
            }
        }

        public void ClearInventoryUI()
        {
            foreach (KeyValuePair<string, InventoryItemUI> slot in _activeUISlots)
            {
                if (slot.Value != null) UnityEngine.Object.Destroy(slot.Value.gameObject);
            }

            _activeUISlots.Clear();
            Debug.Log("[InventoryPanelController] Inventory UI cleared.");
        }
    }
}