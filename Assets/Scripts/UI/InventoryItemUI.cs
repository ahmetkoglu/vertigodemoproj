using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

/// <summary>
/// Controls the visual representation and animations of an individual item slot inside the player's UI inventory.
/// </summary>
public class InventoryItemUI : MonoBehaviour
{
    [Header("Visual Elements")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI amountText;

    private int _currentAmount = 0; 
    private Tween _counterTween; 

    /// <summary>
    /// Initializes the UI slot instantly without any animations. Used upon initial rendering.
    /// </summary>
    /// <param name="itemIcon">The visual icon texture of the reward item.</param>
    /// <param name="startingAmount">The initial base quantity to present.</param>
    public void InitSlot(Sprite itemIcon, int startingAmount)
    {
        icon.sprite = itemIcon;
        _currentAmount = startingAmount;
        amountText.text = "x" + _currentAmount;
    }

    /// <summary>
    /// Animates the item text counter smoothly using an exponential countdown loop
    /// and triggers a localized punch scale bounce effect to enhance visual impact.
    /// </summary>
    /// <param name="targetAmount">The absolute total quantity to tick towards.</param>
    public void AnimateAmount(int targetAmount)
    {
        // Safety: Terminate any active running counters on this slot to prevent data overwrite visual bugs
        _counterTween?.Kill();
        transform.DOKill();

        _counterTween = DOVirtual.Int(_currentAmount, targetAmount, 0.5f, (v) => 
        {
            amountText.text = "x" + v;
        }).SetEase(Ease.OutExpo);
        
        _currentAmount = targetAmount;

        // Visual Juice: Add a brief punchy bounce tracking tactile feedback
        transform.DOPunchScale(Vector3.one * 0.15f, 0.3f, 10, 1f);
    }

    /// <summary>
    /// Introduces the slot item onto the panel overlay using a spring scale transition.
    /// </summary>
    public void Appear()
    {
        transform.DOKill();
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.OutBack);
    }
}