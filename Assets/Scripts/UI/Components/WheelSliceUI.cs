using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WheelGame.Contracts.Rewards;

namespace WheelGame.UI.Components
{
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
            ICollectibleRewardAction collectibleReward = rewardData as ICollectibleRewardAction;
            if (collectibleReward != null)
            {
                amountText.text = collectibleReward.Amount.ToString();
                amountText.gameObject.SetActive(true);
            }
            else
            {
                amountText.gameObject.SetActive(false);
            }
        }
    }
}
}