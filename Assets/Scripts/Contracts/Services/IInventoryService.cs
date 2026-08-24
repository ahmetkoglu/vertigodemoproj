namespace WheelGame.Contracts.Services
{
    public interface IInventoryService
    {
        void AddItem(string itemId, int amount);
        void ClearInventory();
        bool TryGetItemAmount(string itemId, out int amount);
    }
}