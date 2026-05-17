using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Controls the individual graphical interface mapping nodes assigned across concrete sectors of the physical wheel template array.
/// </summary>
public class WheelSliceUI : MonoBehaviour
{
    [Header("Visual Elements")]
    [SerializeField] private Image rewardIcon;
    [SerializeField] private TextMeshProUGUI amountText;

    /// <summary>
    /// Extracts visual properties and configures display labels dynamically using structured identification flags.
    /// </summary>
    /// <param name="rewardData">The raw underlying data instance contract mapped onto this slice container.</param>
    public void Configure(IRewardAction rewardData)
    {
        // 1. Assign the sprite asset texture profile to the display renderer
        rewardIcon.sprite = rewardData.Icon;

        // 2. Evaluate layout identifiers cleanly using our new type-safe property rather than using slow pattern matching syntax
        if (rewardData.Type == RewardType.Bomb)
        {
            amountText.gameObject.SetActive(false); 
        }
        else
        {
            // Safely convert to a data asset structure now that type properties have been explicitly validated
            CollectibleReward collectible = rewardData as CollectibleReward;
            if (collectible != null)
            {
                amountText.text = collectible.amount.ToString();
                amountText.gameObject.SetActive(true);
            }
        }
    }
}