using UnityEngine;
using DG.Tweening;

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
            Debug.LogError("[EvaluationState] Won reward is null! Aborting and forcing InitState.");
            context.ChangeState(new InitState());
            return;
        }

        if (wonReward.Type == RewardType.Bomb)
        {
            ProcessBombOutcome(context);
        }
        else
        {
            ProcessCollectibleOutcome(context, wonReward);
        }
    }

    /// <summary>
    /// Orchestrates the game over sequences when a bomb is processed.
    /// </summary>
    private void ProcessBombOutcome(IGameContext context)
    {
        UnityEngine.UI.Image bgPanel = context.UI.MainBackgroundPanel; 

        UIEffectManager.Instance.PlayBombTensionAnimation(bgPanel, () => 
        {
            context.ChangeState(new GameOverState());
        });
    }

    /// <summary>
    /// Coordinates item allocation, UI slot preparation, and item flight animations for valid rewards.
    /// </summary>
    private void ProcessCollectibleOutcome(IGameContext context, IRewardAction wonReward)
    {
        CollectibleReward collectible = wonReward as CollectibleReward;
        if (collectible == null) return;

        int oldAmount = 0;
        if (context.Inventory.GetAllItems().ContainsKey(collectible.itemId))
        {
            oldAmount = context.Inventory.GetAllItems()[collectible.itemId];
        }

        context.Inventory.AddItem(collectible.itemId, collectible.amount);
        int newAmount = context.Inventory.GetAllItems()[collectible.itemId];

        RectTransform targetSlot = context.UI.PrepareInventorySlot(collectible.itemId, collectible.Icon, oldAmount);
        Vector3 startWorldPos = context.Wheel.GetWinningSlicePosition();
        
        bool isTransitioned = false;

        context.UI.PlayRewardFlightAnimation(collectible.Icon, startWorldPos, targetSlot, () => 
        {
            if (isTransitioned) return;
            isTransitioned = true;

            context.UI.AnimateSlotAmount(collectible.itemId, newAmount);
            context.Zone.IncreaseLevel();
            context.ChangeState(new InitState());
        });

        DOVirtual.DelayedCall(2.0f, () =>
        {
            if (isTransitioned) return;
            isTransitioned = true;

            Debug.LogWarning($"[EvaluationState] Flight animation timed out for {collectible.itemId}. Force-completing progression loop via fail-safe.");
            
            context.UI.AnimateSlotAmount(collectible.itemId, newAmount);
            context.Zone.IncreaseLevel();
            context.ChangeState(new InitState());
        });
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