using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Main Panels")]
    [SerializeField] private Image mainBackgroundPanel;
    public Image MainBackgroundPanel => mainBackgroundPanel;

    [Header("Upcoming Zones UI")]
    [SerializeField] private TextMeshProUGUI txt_nextSafeZone;
    [SerializeField] private TextMeshProUGUI txt_nextSuperZone;
    
    [Header("Popups & Modals")]
    [SerializeField] private GameObject revivePanel;
    [SerializeField] private CanvasGroup exitPopupCanvasGroup;
    [SerializeField] private RectTransform exitPopupBox;

    [Header("VFX & Animation Settings")]    
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private RectTransform inventoryTarget;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventoryContainer;
    [SerializeField] private InventoryItemUI inventoryItemPrefab;
    private Dictionary<string, InventoryItemUI> _activeUISlots = new Dictionary<string, InventoryItemUI>();

    [Header("Progress Bar Settings")]
    [SerializeField] private RectTransform levelContainer;
    [SerializeField] private LevelSlotUI levelSlotPrefab;
    [SerializeField] private float slotWidth = 100f;
    [SerializeField] private int visibleFutureLevels = 10;

    [Header("Buttons: Main Screen")]
    [SerializeField] private Button btn_spin;
    [SerializeField] private Button btn_walk_away;

    [Header("Buttons: Exit Popup")]
    [SerializeField] private Button btn_stay;
    [SerializeField] private Button btn_leave; 

    [Header("Buttons: Revive Popup")]
    [SerializeField] private Button btn_revive;
    [SerializeField] private Button btn_give_up;

    /// <summary>
    /// Assigns all button listeners at the start of the game.
    /// Routes UI interactions directly to the GameManager.
    /// </summary>
    private void Start()
    {
        // 1. Main Screen Buttons
        if (btn_spin != null) 
            btn_spin.onClick.AddListener(GameManager.Instance.OnSpinButtonPressed);
            
        if (btn_walk_away != null) 
            btn_walk_away.onClick.AddListener(ShowExitPopup); 

        // 2. Exit Popup Buttons
        if (btn_stay != null) 
            btn_stay.onClick.AddListener(HideExitPopup); 

        if (btn_leave != null)
        {
            btn_leave.onClick.AddListener(() => {
                HideExitPopup();
                GameManager.Instance.OnWalkAwayButtonPressed(); 
            });
        }

        // 3. Revive (Bomb) Popup Buttons
        if (btn_revive != null)
        {
            btn_revive.onClick.AddListener(() => {
                ShowRevivePanel(false); 
                GameManager.Instance.OnReviveButtonPressed(); 
            });
        }

        if (btn_give_up != null)
        {
            btn_give_up.onClick.AddListener(() => {
                ShowRevivePanel(false); 
                GameManager.Instance.OnGiveUpButtonPressed(); 
            });
        }
    }

    /// <summary>
    /// Removes all listeners when the object is destroyed to prevent memory leaks.
    /// </summary>
    private void OnDestroy()
    {
        if (btn_spin != null) btn_spin.onClick.RemoveAllListeners();
        if (btn_walk_away != null) btn_walk_away.onClick.RemoveAllListeners();
        if (btn_stay != null) btn_stay.onClick.RemoveAllListeners();
        if (btn_leave != null) btn_leave.onClick.RemoveAllListeners();
        if (btn_revive != null) btn_revive.onClick.RemoveAllListeners();
        if (btn_give_up != null) btn_give_up.onClick.RemoveAllListeners();
    }

    /// <summary>
    /// Enables or disables the interaction of the Spin button.
    /// </summary>
    /// <param name="isActive">True to enable, false to disable.</param>
    public void SetSpinButtonState(bool isActive)
    {
        if (btn_spin != null) btn_spin.interactable = isActive;
    }

    /// <summary>
    /// Enables or disables the interaction of the Walk Away button.
    /// </summary>
    /// <param name="isActive">True to enable, false to disable.</param>
    public void SetWalkAwayButtonState(bool isActive)
    {
        if (btn_walk_away != null) btn_walk_away.interactable = isActive;
    }

    /// <summary>
    /// Initializes the level progress bar with upcoming zones.
    /// </summary>
    /// <param name="currentLevel">The starting level index.</param>
    public void InitProgressBar(int currentLevel)
    {
        for (int i = 0; i < visibleFutureLevels; i++)
        {
            CreateLevelSlot(currentLevel + i);
        }
    }

    /// <summary>
    /// Instantiates a single level slot and configures its colors based on zone type.
    /// </summary>
    /// <param name="level">The level number to display.</param>
    private void CreateLevelSlot(int level)
    {
        LevelSlotUI newSlot = Instantiate(levelSlotPrefab, levelContainer);
        bool isSuper = level % 30 == 0;
        bool isSafe = level % 5 == 0 && !isSuper;
        newSlot.Configure(level, isSafe, isSuper);
    }

    /// <summary>
    /// Animates the progress bar sliding to the left to show the current level.
    /// </summary>
    /// <param name="currentLevel">The current level the player is on.</param>
    public void UpdateLevelProgress(int currentLevel)
    {
        if (levelContainer == null) return;

        LayoutRebuilder.ForceRebuildLayoutImmediate(levelContainer);
        float targetX = -(currentLevel - 1) * slotWidth;

        levelContainer.DOKill();
        levelContainer.DOAnchorPosX(targetX, 0.6f).SetEase(Ease.OutBack);
    }

    /// <summary>
    /// Prepares an inventory slot for a new item. Creates a new slot if it doesn't exist.
    /// </summary>
    /// <param name="itemId">The unique ID of the item.</param>
    /// <param name="icon">The sprite icon of the item.</param>
    /// <param name="currentAmount">The current quantity of the item before adding.</param>
    /// <returns>The RectTransform of the target slot for animation purposes.</returns>
    public RectTransform PrepareInventorySlot(string itemId, Sprite icon, int currentAmount)
    {
        if (_activeUISlots.ContainsKey(itemId))
        {
            return _activeUISlots[itemId].GetComponent<RectTransform>();
        }
        
        InventoryItemUI newSlot = Instantiate(inventoryItemPrefab, inventoryContainer);
        newSlot.InitSlot(icon, currentAmount);
        newSlot.Appear(); 
        
        _activeUISlots.Add(itemId, newSlot);
        LayoutRebuilder.ForceRebuildLayoutImmediate(inventoryContainer as RectTransform);
        
        return newSlot.GetComponent<RectTransform>();
    }

    /// <summary>
    /// Triggers the text animation on an inventory slot to show the updated amount.
    /// </summary>
    /// <param name="itemId">The unique ID of the item.</param>
    /// <param name="newAmount">The updated total amount.</param>
    public void AnimateSlotAmount(string itemId, int newAmount)
    {
        if (_activeUISlots.ContainsKey(itemId))
        {
            _activeUISlots[itemId].AnimateAmount(newAmount);
        }
    }

    /// <summary>
    /// Plays the flying animation for a collected reward from the wheel to the inventory.
    /// </summary>
    /// <param name="rewardIcon">The icon of the collected reward.</param>
    /// <param name="startWorldPos">The starting world position (usually on the wheel).</param>
    /// <param name="targetSlot">The destination RectTransform in the UI.</param>
    /// <param name="onComplete">Callback executed when the flight animation ends.</param>
    public void PlayRewardFlightAnimation(Sprite rewardIcon, Vector3 startWorldPos, RectTransform targetSlot, System.Action onComplete)
    {
        UIEffectManager.Instance.PlayCollectAnimation(mainCanvas, rewardIcon, startWorldPos, targetSlot, onComplete);
    }

    /// <summary>
    /// Clears all items from the visual inventory UI. Usually called upon game over.
    /// </summary>
    public void ClearInventoryUI()
    {
        foreach (var slot in _activeUISlots.Values)
        {
            if (slot != null) Destroy(slot.gameObject);
        }
        
        _activeUISlots.Clear();
        Debug.Log("[UIManager] Inventory UI cleared.");
    }

    /// <summary>
    /// Shows or hides the revive panel when a bomb is hit.
    /// </summary>
    /// <param name="isActive">True to show, false to hide.</param>
    public void ShowRevivePanel(bool isActive)
    {
        if (revivePanel != null)
        {
            revivePanel.SetActive(isActive);
            if (isActive)
            {
                revivePanel.transform.localScale = Vector3.zero;
                revivePanel.transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.OutBack);
            }
        }
    }

    /// <summary>
    /// Updates the text indicators for the upcoming safe and super zones using pre-calculated values.
    /// </summary>
    /// <param name="nextSafeLevel">The pre-calculated level index of the next safe zone.</param>
    /// <param name="nextSuperLevel">The pre-calculated level index of the next super zone.</param>
    public void UpdateUpcomingZones(int nextSafeLevel, int nextSuperLevel)
    {
        if (txt_nextSafeZone != null) 
            txt_nextSafeZone.text = $"Safe Zone: {nextSafeLevel}";
            
        if (txt_nextSuperZone != null) 
            txt_nextSuperZone.text = $"Super Zone: {nextSuperLevel}";
    }

    /// <summary>
    /// Opens the Exit (Walk Away) confirmation popup with a fade and scale animation.
    /// </summary>
    public void ShowExitPopup()
    {
        exitPopupCanvasGroup.gameObject.SetActive(true);
        exitPopupCanvasGroup.alpha = 0f;
        exitPopupBox.localScale = Vector3.zero;

        exitPopupCanvasGroup.DOFade(1f, 0.2f);
        exitPopupBox.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }

    /// <summary>
    /// Closes the Exit (Walk Away) confirmation popup with a shrink animation.
    /// </summary>
    public void HideExitPopup()
    {
        exitPopupBox.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack).OnComplete(() => 
        {
            exitPopupCanvasGroup.alpha = 0f;
            exitPopupCanvasGroup.gameObject.SetActive(false);
        });
    }

#if UNITY_EDITOR
    /// <summary>
    /// Automatically assigns button references in the Unity Editor based on object names.
    /// </summary>
    private void OnValidate()
    {
        Button[] allButtons = Resources.FindObjectsOfTypeAll<Button>();

        foreach (Button btn in allButtons)
        {
            switch (btn.gameObject.name)
            {
                case "ui_button_spin": if (btn_spin == null) btn_spin = btn; break;
                case "ui_button_exit": if (btn_walk_away == null) btn_walk_away = btn; break;
                case "ui_button_stay": if (btn_stay == null) btn_stay = btn; break;
                case "ui_button_leave": if (btn_leave == null) btn_leave = btn; break;
                case "ui_button_revive": if (btn_revive == null) btn_revive = btn; break;
                case "ui_button_giveup": if (btn_give_up == null) btn_give_up = btn; break;
            }
        }
    }
#endif
}