using System.Collections.Generic;
using UnityEngine;
using WheelGame.Contracts.Rewards;
using WheelGame.Gameplay.Rewards;

namespace WheelGame.Gameplay.Progression
{
[CreateAssetMenu(fileName = "NewZoneData", menuName = "GameData/Zone Pool")]
public class ZoneData : ScriptableObject
{
    #region Visual Configuration

    [Header("Visuals")]
    public Sprite wheelSprite;      
    public Sprite indicatorSprite;  

    #endregion

    #region Rule Configuration
    
    [Header("Rules")]
    [Tooltip("Can a bomb appear in this zone pool?")]
    public bool hasBomb;
    
    [Tooltip("The bomb reward configuration added to the wheel if hasBomb is true")]
    public BombReward bombData;

    #endregion

    #region Reward Pool Configuration

    [Header("Reward Pool")]
    [Tooltip("Drag and drop CollectibleReward configuration assets here")]
    public List<CollectibleReward> availableRewards;

    #endregion

    #region Pool Generation
    
    /// <summary>
    /// Generates a randomized list of 8 rewards (slices) filling the wheel architecture based on zone rules.
    /// Includes a bomb if configured, and shuffles the output using Fisher-Yates.
    /// </summary>
    /// <returns>A shuffled list of 8 IRewardAction elements for the wheel display.</returns>
    public List<IRewardAction> GenerateWheelPool()
    {
        List<IRewardAction> finalPool = new List<IRewardAction>();
        int totalSlices = 8; 

        // 1. If the zone contains a bomb, append the bomb data into the first slot
        if (hasBomb && bombData != null)
        {
            finalPool.Add(bombData);
        }

        // 2. Validate reward configurations to prevent runtime null-reference crashes
        if (availableRewards != null && availableRewards.Count > 0)
        {
            // Calculate remaining empty spaces (7 spaces if bomb exists, 8 if not)
            int slotsToFill = totalSlices - finalPool.Count;

            for (int i = 0; i < slotsToFill; i++)
            {
                // Grab a random configuration from the available asset pool
                int randomIndex = Random.Range(0, availableRewards.Count);
                finalPool.Add(availableRewards[randomIndex]);
            }
        }
        else
        {
            Debug.LogError($"[ZoneData] 'Available Rewards' list is completely empty in {this.name}! Please assign rewards in the Inspector.");
        }

        // 3. Shuffle the distribution array so the bomb position randomizes away from index 0
        ShuffleList(finalPool);

        return finalPool;
    }

    /// <summary>
    /// Implements a basic Fisher-Yates shuffle algorithm to completely randomize the list elements in-place.
    /// </summary>
    /// <param name="list">The target list configuration to randomize.</param>
    private void ShuffleList(List<IRewardAction> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            IRewardAction temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    #endregion

#if UNITY_EDITOR
    #region Editor Validation

    private void OnValidate()
    {
        if (wheelSprite == null)
        {
            Debug.LogWarning($"[ZoneData] wheelSprite is missing in asset '{name}'.", this);
        }

        if (indicatorSprite == null)
        {
            Debug.LogWarning($"[ZoneData] indicatorSprite is missing in asset '{name}'.", this);
        }

        if (hasBomb && bombData == null)
        {
            Debug.LogWarning($"[ZoneData] hasBomb is enabled but bombData is not assigned in asset '{name}'.", this);
        }

        if (availableRewards == null || availableRewards.Count == 0)
        {
            Debug.LogWarning($"[ZoneData] availableRewards is empty in asset '{name}'.", this);
        }
    }

    #endregion
#endif
}
}