using UnityEngine;
using WheelGame.Contracts.Rewards;
using WheelGame.Contracts.StateMachine;

namespace WheelGame.Gameplay.StateMachine.States
{
/// <summary>
/// State responsible for evaluating the outcome of the wheel spin.
/// Inspects the won reward type and coordinates inventory updates, visual animations, and subsequent state transitions.
/// </summary>
public class EvaluationState : IGameState
{
    private readonly int _selectedIndex;

    /// <summary>
    /// Initializes a new instance of the EvaluationState with the index of the winning wheel slice.
    /// </summary>
    /// <param name="index">The selected slice index on the wheel.</param>
    public EvaluationState(int index)
    {
        _selectedIndex = index;
    }

    /// <summary>
    /// Executed automatically when entering the evaluation state.
    /// Fetches the reward data and routes the game flow based on whether it's a bomb or a collectible.
    /// </summary>
    /// <param name="context">The central game context providing access to managers.</param>
    public void EnterState(IGameContext context)
    {
        Debug.Log($"[EvaluationState] Analyzing slice index: {_selectedIndex}...");

        IRewardAction wonReward = context.Zone.GetRewardAtIndex(_selectedIndex);

        if (wonReward == null)
        {
            Debug.LogWarning("[EvaluationState] Won reward is null! Aborting and forcing InitState.");
            context.ChangeState(new InitState());
            return;
        }

        context.RewardResolver.Resolve(wonReward, context);
    }

    /// <summary>
    /// Executed automatically when exiting the evaluation state.
    /// </summary>
    /// <param name="context">The central game context providing access to managers.</param>
    public void ExitState(IGameContext context)
    {
        Debug.Log("[EvaluationState] Outcome analysis finalized.");
    }
}
}