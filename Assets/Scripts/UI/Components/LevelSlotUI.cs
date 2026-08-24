using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace WheelGame.UI.Components
{
/// <summary>
/// Represents a individual cell entry within the horizontal level progression tracking layout array.
/// </summary>
public class LevelSlotUI : MonoBehaviour
{
    [Header("Visual Components")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Image backgroundImage;

    [Header("Designer Colors")]
    [SerializeField] private Color normalZoneColor = Color.white;
    [SerializeField] private Color safeZoneColor = Color.green;
    [SerializeField] private Color superZoneColor = new Color(1f, 0.84f, 0f); // Pure Gold

    /// <summary>
    /// Updates layout text readouts and adjusts underlying backdrop color channels based on active zone metrics.
    /// </summary>
    /// <param name="level">The absolute level index number.</param>
    /// <param name="isSafe">Does this index evaluate into a safe zone criteria pattern?</param>
    /// <param name="isSuper">Does this index evaluate into a super zone criteria pattern?</param>
    public void Configure(int level, bool isSafe, bool isSuper)
    {
        levelText.text = level.ToString();
        
        // Map logical flags cleanly directly onto visual color properties
        if (isSuper) 
            backgroundImage.color = superZoneColor;
        else if (isSafe) 
            backgroundImage.color = safeZoneColor;
        else 
            backgroundImage.color = normalZoneColor;
    }
}
}