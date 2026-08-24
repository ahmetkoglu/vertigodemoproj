using System.Collections.Generic;
using WheelGame.Contracts.Services;

namespace WheelGame.Tests.EditMode.TestDoubles
{
    public class FakeInventoryService : IInventoryService
    {
        private readonly Dictionary<string, int> _items = new Dictionary<string, int>();

        public bool ClearCalled { get; private set; }

        public void AddItem(string itemId, int amount)
        {
            if (_items.ContainsKey(itemId))
            {
                _items[itemId] += amount;
            }
            else
            {
                _items[itemId] = amount;
            }
        }

        public void ClearInventory()
        {
            ClearCalled = true;
            _items.Clear();
        }

        public bool TryGetItemAmount(string itemId, out int amount)
        {
            return _items.TryGetValue(itemId, out amount);
        }
    }
}