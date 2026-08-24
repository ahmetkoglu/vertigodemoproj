using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using WheelGame.Contracts.Services;
using WheelGame.UI.Components;
using WheelGame.UI.Controllers;
using WheelGame.UI.Effects;

namespace WheelGame.UI
{
public class UIManager : MonoBehaviour, IGameUI
{
    #region Events

    public event Action SpinRequested;
    public event Action WalkAwayRequested;
    public event Action ReviveRequested;
    public event Action GiveUpRequested;

    #endregion

    #region Serialized References

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
    [SerializeField] private UIEffectManager uiEffectManager;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventoryContainer;
    [SerializeField] private InventoryItemUI inventoryItemPrefab;

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

    #endregion

    #region Runtime Controllers

    private UIInputController _inputController;
    private InventoryPanelController _inventoryPanelController;
    private ProgressionPanelController _progressionPanelController;
    private OverlayUIController _overlayUIController;
    private RewardAnimationController _rewardAnimationController;

    #endregion

    #region Unity Lifecycle

    private void Awake()
    {
        _overlayUIController = new OverlayUIController(revivePanel, exitPopupCanvasGroup, exitPopupBox);
        _inventoryPanelController = new InventoryPanelController(inventoryContainer, inventoryItemPrefab);
        _progressionPanelController = new ProgressionPanelController(txt_nextSafeZone, txt_nextSuperZone, levelContainer, levelSlotPrefab, slotWidth, visibleFutureLevels);
        _rewardAnimationController = new RewardAnimationController(mainCanvas, mainBackgroundPanel, uiEffectManager);
        _inputController = new UIInputController(
            btn_spin,
            btn_walk_away,
            btn_stay,
            btn_leave,
            btn_revive,
            btn_give_up,
            () => SpinRequested?.Invoke(),
            () => WalkAwayRequested?.Invoke(),
            () => ReviveRequested?.Invoke(),
            () => GiveUpRequested?.Invoke(),
            _overlayUIController.ShowExitPopup,
            _overlayUIController.HideExitPopup,
            _overlayUIController.ShowRevivePanel);
    }

    /// <summary>
    /// Assigns all button listeners at the start of the game.
    /// Routes UI interactions directly to the GameManager.
    /// </summary>
    private void Start()
    {
        _inputController?.Bind();
    }

    /// <summary>
    /// Removes all listeners when the object is destroyed to prevent memory leaks.
    /// </summary>
    private void OnDestroy()
    {
        _inputController?.Unbind();
    }

    #endregion

    #region IInputControlUI

    /// <summary>
    /// Enables or disables the interaction of the Spin button.
    /// </summary>
    /// <param name="isActive">True to enable, false to disable.</param>
    public void SetSpinButtonState(bool isActive)
    {
        _inputController?.SetSpinButtonState(isActive);
    }

    /// <summary>
    /// Enables or disables the interaction of the Walk Away button.
    /// </summary>
    /// <param name="isActive">True to enable, false to disable.</param>
    public void SetWalkAwayButtonState(bool isActive)
    {
        _inputController?.SetWalkAwayButtonState(isActive);
    }

    #endregion

    #region IProgressionUI

    /// <summary>
    /// Initializes the level progress bar with upcoming zones.
    /// </summary>
    /// <param name="currentLevel">The starting level index.</param>
    public void InitProgressBar(int currentLevel)
    {
        _progressionPanelController?.InitProgressBar(currentLevel);
    }

    /// <summary>
    /// Animates the progress bar sliding to the left to show the current level.
    /// </summary>
    /// <param name="currentLevel">The current level the player is on.</param>
    public void UpdateLevelProgress(int currentLevel)
    {
        _progressionPanelController?.UpdateLevelProgress(currentLevel);
    }

    /// <summary>
    /// Updates the text indicators for the upcoming safe and super zones using pre-calculated values.
    /// </summary>
    /// <param name="nextSafeLevel">The pre-calculated level index of the next safe zone.</param>
    /// <param name="nextSuperLevel">The pre-calculated level index of the next super zone.</param>
    public void UpdateUpcomingZones(int nextSafeLevel, int nextSuperLevel)
    {
        _progressionPanelController?.UpdateUpcomingZones(nextSafeLevel, nextSuperLevel);
    }

    #endregion

    #region IInventoryUI

    /// <summary>
    /// Prepares an inventory slot for a new item. Creates a new slot if it doesn't exist.
    /// </summary>
    /// <param name="itemId">The unique ID of the item.</param>
    /// <param name="icon">The sprite icon of the item.</param>
    /// <param name="currentAmount">The current quantity of the item before adding.</param>
    /// <returns>The RectTransform of the target slot for animation purposes.</returns>
    public RectTransform PrepareInventorySlot(string itemId, Sprite icon, int currentAmount)
    {
        return _inventoryPanelController != null
            ? _inventoryPanelController.PrepareInventorySlot(itemId, icon, currentAmount)
            : null;
    }

    /// <summary>
    /// Triggers the text animation on an inventory slot to show the updated amount.
    /// </summary>
    /// <param name="itemId">The unique ID of the item.</param>
    /// <param name="newAmount">The updated total amount.</param>
    public void AnimateSlotAmount(string itemId, int newAmount)
    {
        _inventoryPanelController?.AnimateSlotAmount(itemId, newAmount);
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
        _rewardAnimationController?.PlayRewardFlightAnimation(rewardIcon, startWorldPos, targetSlot, onComplete);
    }

    public void PlayBombTensionAnimation(System.Action onComplete)
    {
        _rewardAnimationController?.PlayBombTensionAnimation(onComplete);
    }

    /// <summary>
    /// Clears all items from the visual inventory UI. Usually called upon game over.
    /// </summary>
    public void ClearInventoryUI()
    {
        _inventoryPanelController?.ClearInventoryUI();
    }

    #endregion

    #region IOverlayUI

    /// <summary>
    /// Shows or hides the revive panel when a bomb is hit.
    /// </summary>
    /// <param name="isActive">True to show, false to hide.</param>
    public void ShowRevivePanel(bool isActive)
    {
        _overlayUIController?.ShowRevivePanel(isActive);
    }

    /// <summary>
    /// Opens the Exit (Walk Away) confirmation popup with a fade and scale animation.
    /// </summary>
    public void ShowExitPopup()
    {
        _overlayUIController?.ShowExitPopup();
    }

    /// <summary>
    /// Closes the Exit (Walk Away) confirmation popup with a shrink animation.
    /// </summary>
    public void HideExitPopup()
    {
        _overlayUIController?.HideExitPopup();
    }

    #endregion

#if UNITY_EDITOR
    #region Editor Validation

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

    #endregion
#endif
}
}