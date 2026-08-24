using UnityEngine;
using WheelGame.Contracts.StateMachine;

namespace WheelGame.Gameplay.StateMachine.States
{
/// <summary>
/// State representing the stagnant waiting phase where the core loop pauses for explicit user interaction.
/// Safely manages active button interactivity metrics.
/// </summary>
public class IdleState : IGameState
{
    /// <summary>
    /// Executed automatically when entering the idle state. Unlocks controls based on strict validation rules.
    /// </summary>
    /// <param name="context">The central game context providing access to managers.</param>
    public void EnterState(IGameContext context)
    {
        Debug.Log("[IdleState] Ready and awaiting player decision...");
        
        // Ensure core input mechanisms are functional
        context.InputUI.SetSpinButtonState(true);

        // Check zone safety structures to determine if a walk-away extraction is valid
        bool canClaim = context.Zone.IsSafeZone || context.Zone.IsSuperZone;
        context.InputUI.SetWalkAwayButtonState(canClaim);
    }

    /// <summary>
    /// Executed automatically when leaving the idle state. Locks down interactive systems to eliminate input spam risks.
    /// </summary>
    /// <param name="context">The central game context providing access to managers.</param>
    public void ExitState(IGameContext context)
    {
        Debug.Log("[IdleState] Input registered! Securely locking interface buttons.");
        
        context.InputUI.SetSpinButtonState(false);
        context.InputUI.SetWalkAwayButtonState(false);
    }
}
}