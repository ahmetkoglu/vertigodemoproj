using UnityEngine;

/// <summary>
/// State responsible for initializing the current zone configuration,
/// setting up wheel rewards, and preparing layout progression views before player input.
/// </summary>
public class InitState : IGameState
{
    /// <summary>
    /// Automatically executed when entering the initialization state.
    /// Synchronizes visual wheel graphics, reward parameters, and sliding progress bar states.
    /// </summary>
    /// <param name="context">The central game context providing access to managers.</param>
    public void EnterState(IGameContext context)
    {
        // 1. Fetch data configuration from the progression manager
        ZoneData currentConfig = context.Zone.GetCurrentZoneData();
        bool isSafe = context.Zone.IsSafeZone;
        bool isSuper = context.Zone.IsSuperZone;

        // 2. Dispatch structural textures and layout parameters directly to the wheel
        context.Wheel.UpdateWheelVisuals(currentConfig.wheelSprite, currentConfig.indicatorSprite, isSafe, isSuper);
        
        Debug.Log($"[InitState] Preparing Zone Level: {context.Zone.CurrentLevel}...");

        // 3. Rebuild reward datasets and assemble visual slices
        var generatedRewards = context.Zone.GenerateNewWheel();
        context.Wheel.SetupWheel(generatedRewards);

        // 4. Gather layout integers and notify tracking displays
        int nextSafeLevel = context.Zone.GetNextSafeZoneLevel();
        int nextSuperLevel = context.Zone.GetNextSuperZoneLevel();
        context.UI.UpdateUpcomingZones(nextSafeLevel, nextSuperLevel);

        // 5. Evaluate layout progression ticks for the horizontal slider tracker
        if (context.Zone.CurrentLevel > 1)
        {
            context.UI.UpdateLevelProgress(context.Zone.CurrentLevel);
        }
        else 
        {
            context.UI.InitProgressBar(1);
            context.UI.UpdateLevelProgress(1);
        }

        // Processing finished seamlessly, immediately hand control off to input monitoring
        context.ChangeState(new IdleState());
    }

    public void ExitState(IGameContext context) { }
}