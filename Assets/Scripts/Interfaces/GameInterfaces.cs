using UnityEngine;

/// <summary>
/// Contract for all game states in the State Machine.
/// Defines the lifecycle methods that occur when entering and exiting a state.
/// </summary>
public interface IGameState
{
    /// <summary>
    /// Called automatically when the state machine transitions into this state.
    /// </summary>
    /// <param name="context">The central game context providing access to managers.</param>
    void EnterState(IGameContext context);

    /// <summary>
    /// Called automatically when the state machine transitions out of this state.
    /// </summary>
    /// <param name="context">The central game context providing access to managers.</param>
    void ExitState(IGameContext context);
}

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
    
    // --- Core Sub-Managers ---
    UIManager UI { get; }
    WheelManager Wheel { get; }
    ZoneManager Zone { get; }
    InventoryManager Inventory { get; }
}

/// <summary>
/// Contract for all rewards generated on the wheel.
/// Combines the visual representation with the execution logic.
/// </summary>
public interface IRewardAction
{
    /// <summary>
    /// The visual sprite displayed on the wheel slice.
    /// </summary>
    Sprite Icon { get; } 
    RewardType Type { get; } // Added to identify the reward type cleanly without typecasting
    
    /// <summary>
    /// The action executed when the wheel stops on this reward.
    /// </summary>
    /// <param name="context">The game context used to apply the reward effects.</param>
}