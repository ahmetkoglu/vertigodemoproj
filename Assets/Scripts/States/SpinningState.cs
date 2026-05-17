using UnityEngine;

/// <summary>
/// State tracking active physical execution of the reward wheel asset rotation mechanics.
/// Intercepts completion callbacks before triggering structural balance checks.
/// </summary>
public class SpinningState : IGameState
{
    private IGameContext _context;

    /// <summary>
    /// Executed automatically when entering the spinning phase. Caches context references and subscribes to events.
    /// </summary>
    /// <param name="context">The central game context providing access to managers.</param>
    public void EnterState(IGameContext context)
    {
        Debug.Log("[SpinningState] Wheel sequence initiated.");
        
        // Cache the incoming framework reference to handle async callbacks safely without global singleton calls
        _context = context;
        
        // Establish listeners on mechanical finish parameters
        _context.Wheel.OnSpinComplete += HandleSpinComplete;
        int targetIndex = UnityEngine.Random.Range(0, 8); 
        _context.Wheel.SpinWheel(targetIndex);
    }

    /// <summary>
    /// Async handler executed automatically when rotation physics complete. Passes outcome details to evaluation pipelines.
    /// </summary>
    /// <param name="index">The landed data sector indexing marker.</param>
    private void HandleSpinComplete(int index)
    {
        // FIXED: Now accurately utilizes the cached context contract rather than bypassing via Singleton shortcuts
        _context.ChangeState(new EvaluationState(index));
    }

    /// <summary>
    /// Executed automatically when leaving the spin cycle. Cleans active subscriptions to eliminate memory footprint risks.
    /// </summary>
    /// <param name="context">The central game context providing access to managers.</param>
    public void ExitState(IGameContext context)
    {
        context.Wheel.OnSpinComplete -= HandleSpinComplete;
        Debug.Log("[SpinningState] Rotation timeline terminated.");
    }
}