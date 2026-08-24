using UnityEngine;
using WheelGame.Contracts.StateMachine;

namespace WheelGame.Gameplay.StateMachine.States
{
/// <summary>
/// State triggered immediately when a failure criteria (bomb impact) registers without passive shields.
/// Holds gameplay interaction and renders recovery prompts.
/// </summary>
public class GameOverState : IGameState
{
    /// <summary>
    /// Executed automatically when failure patterns activate. Forces standard ui components offline and presents recovery panels.
    /// </summary>
    /// <param name="context">The central game context providing access to managers.</param>
    public void EnterState(IGameContext context)
    {
        Debug.Log("[GameOverState] Triggering defeat sequencing and rendering recovery screens.");

        // Lock standard interaction nodes to protect back-end processing arrays
        context.InputUI.SetSpinButtonState(false);
        context.InputUI.SetWalkAwayButtonState(false);

        // Bring the canvas module visual asset overlays into view
        context.OverlayUI.ShowRevivePanel(true);
    }

    /// <summary>
    /// Executed automatically when resolution criteria close the failure window (via revival checkout or forfeit clear loops).
    /// </summary>
    /// <param name="context">The central game context providing access to managers.</param>
    public void ExitState(IGameContext context)
    {
        // Dismantle overlay canvases completely as control passes back to active workflows
        context.OverlayUI.ShowRevivePanel(false);
    }
}
}