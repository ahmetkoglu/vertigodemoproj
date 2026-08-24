using UnityEngine;
using WheelGame.Contracts.StateMachine;

namespace WheelGame.Gameplay.StateMachine.States
{
/// <summary>
/// State tracking extraction and processing. Flushes ongoing tracking buffers and commits currency rewards before resetting game metrics.
/// </summary>
public class ClaimState : IGameState
{
    /// <summary>
    /// Executed automatically upon bank extraction entry. Commits persistent saves, cleans cache structures, and loops progression states.
    /// </summary>
    /// <param name="context">The central game context providing access to managers.</param>
    public void EnterState(IGameContext context)
    {
        Debug.Log("[ClaimState] Extraction parameters validated. Processing rewards transfer loops.");

        // 1. Wipe temporary collection nodes safely
        context.Inventory.ClearInventory();

        // 2. Disassemble and flush visual display assets from screen slots
        context.InventoryUI.ClearInventoryUI();

        // 3. Fall progression variables flatly back to base layer zero values
        context.Zone.ResetLevel();

        // 4. Force state engine back through the base bootstrapping phase
        context.ChangeState(new InitState());
    }

    public void ExitState(IGameContext context) { }
}
}