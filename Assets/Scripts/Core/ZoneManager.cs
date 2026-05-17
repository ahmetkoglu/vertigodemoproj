using System.Collections.Generic;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    [Header("Zone Data Pools")]
    [SerializeField] private ZoneData normalZoneData;
    [SerializeField] private ZoneData safeZoneData;
    [SerializeField] private ZoneData superZoneData;
    
    private List<IRewardAction> _currentPool;
    private int _currentLevel = 1;

    // --- Properties ---
    public int CurrentLevel => _currentLevel;
    public bool IsSuperZone => _currentLevel % 30 == 0;
    public bool IsSafeZone => _currentLevel % 5 == 0 && !IsSuperZone;

    // --- Core Actions ---

    /// <summary>
    /// Advances the game to the next zone level.
    /// </summary>
    public void IncreaseLevel()
    {
        _currentLevel++;
        Debug.Log($"[ZoneManager] Level Increased! Current Level: {_currentLevel}");
    }

    /// <summary>
    /// Resets the progression back to level 1. Called when the player gives up or completes the game.
    /// </summary>
    public void ResetLevel()
    {
        _currentLevel = 1;
        Debug.Log("[ZoneManager] Progression reset to Level 1.");
    }

    /// <summary>
    /// Determines the appropriate zone data (Normal, Safe, Super) based on current progression.
    /// </summary>
    /// <returns>The ZoneData ScriptableObject for the current level.</returns>
    public ZoneData GetCurrentZoneData()
    {
        if (IsSuperZone) return superZoneData;
        if (IsSafeZone) return safeZoneData;
        return normalZoneData; 
    }

    /// <summary>
    /// Generates a fresh pool of rewards (8 slices) for the wheel based on current zone rules.
    /// </summary>
    /// <returns>A list of IRewardAction representing the wheel slices.</returns>
    public List<IRewardAction> GenerateNewWheel()
    {
        ZoneData currentZone = GetCurrentZoneData();
        _currentPool = currentZone.GenerateWheelPool(); 
        return _currentPool;
    }

    /// <summary>
    /// Retrieves a specific reward from the current active pool.
    /// </summary>
    public IRewardAction GetRewardAtIndex(int index)
    {
        return _currentPool[index];
    }

    // --- Upcoming Zone Calculations ---

    /// <summary>
    /// Calculates the level index of the next safe zone.
    /// </summary>
    public int GetNextSafeZoneLevel()
    {
        int nextSafe = ((_currentLevel / 5) + 1) * 5;
        if (nextSafe % 30 == 0) nextSafe += 5; // Skip super zones
        return nextSafe;
    }

    /// <summary>
    /// Calculates the level index of the next super zone.
    /// </summary>
    public int GetNextSuperZoneLevel()
    {
        return ((_currentLevel / 30) + 1) * 30;
    }
}