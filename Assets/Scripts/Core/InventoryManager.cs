using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // Dictionary holding the accumulated items. Key: Item ID, Value: Amount
    private Dictionary<string, int> _earnedItems = new Dictionary<string, int>();

    /// <summary>
    /// Adds a specified amount of an item to the temporary inventory.
    /// </summary>
    /// <param name="itemId">The unique identifier of the item.</param>
    /// <param name="amount">The quantity to add.</param>
    public void AddItem(string itemId, int amount)
    {
        if (_earnedItems.ContainsKey(itemId))
        {
            _earnedItems[itemId] += amount;
        }
        else
        {
            _earnedItems.Add(itemId, amount);
        }

        Debug.Log($"[InventoryManager] Added: {itemId} +{amount}. Total: {_earnedItems[itemId]}");
    }

    /// <summary>
    /// Wipes all accumulated items from the inventory. Usually called upon game over.
    /// </summary>
    public void ClearInventory()
    {
        _earnedItems.Clear();
        Debug.Log("[InventoryManager] Inventory has been cleared.");
    }

    /// <summary>
    /// Returns the complete dictionary of currently earned items.
    /// </summary>
    public Dictionary<string, int> GetAllItems() => _earnedItems;
}