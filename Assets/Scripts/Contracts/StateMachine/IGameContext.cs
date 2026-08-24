using WheelGame.Contracts.Services;
using WheelGame.Contracts.Rewards;

namespace WheelGame.Contracts.StateMachine
{
    /// <summary>
    /// Central context contract used by states to interact with the game world.
    /// Applies Dependency Inversion so states depend on abstractions, not concrete managers.
    /// </summary>
    public interface IGameContext
    {
        /// <summary>
        /// Requests a transition to a new game state.
        /// </summary>
        /// <param name="newState">The new state to transition to.</param>
        void ChangeState(IGameState newState);

        IInputControlUI InputUI { get; }
        IInventoryUI InventoryUI { get; }
        IOverlayUI OverlayUI { get; }
        IProgressionUI ProgressionUI { get; }
        IWheelService Wheel { get; }
        IZoneService Zone { get; }
        IInventoryService Inventory { get; }
        IRewardFlowService RewardFlow { get; }
        IRewardResolver RewardResolver { get; }
    }
}