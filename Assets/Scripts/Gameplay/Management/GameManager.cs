using UnityEngine;
using WheelGame.Contracts.Services;
using WheelGame.Contracts.StateMachine;
using WheelGame.Contracts.Rewards;
using WheelGame.Gameplay.Inventory;
using WheelGame.Gameplay.Progression;
using WheelGame.Gameplay.Rewards.Resolution;
using WheelGame.Gameplay.StateMachine.States;
using WheelGame.Gameplay.Wheel;
using WheelGame.UI;

namespace WheelGame.Gameplay.Management
{
public class GameManager : MonoBehaviour, IGameContext
{
    #region Serialized References

    [Header("Sub-Managers")]
    [SerializeField] private UIManager uiManager;
    [SerializeField] private WheelManager wheelManager;
    [SerializeField] private ZoneManager zoneManager;
    [SerializeField] private InventoryManager inventoryManager;

    #endregion

    #region IGameContext

    // IGameContext Implementations
    public IInputControlUI InputUI => uiManager;
    public IInventoryUI InventoryUI => uiManager;
    public IOverlayUI OverlayUI => uiManager;
    public IProgressionUI ProgressionUI => uiManager;
    public IWheelService Wheel => wheelManager;
    public IZoneService Zone => zoneManager;
    public IInventoryService Inventory => inventoryManager;
    public IRewardFlowService RewardFlow { get; private set; }
    public IRewardResolver RewardResolver { get; private set; }

    #endregion

    #region Runtime Services

    private GameStateMachine _stateMachine;
    private GameCommandCoordinator _commandCoordinator;

    #endregion

    #region Unity Lifecycle

    private void Awake()
    {
        Application.targetFrameRate = 144;
        QualitySettings.vSyncCount = 0;

        RewardResolver = new RewardResolver(new IRewardResolutionHandler[]
        {
            new BombRewardResolutionHandler(),
            new CollectibleRewardResolutionHandler()
        });

        _stateMachine = new GameStateMachine(this);
        RewardFlow = new GameRewardFlowController(_stateMachine);
        _commandCoordinator = new GameCommandCoordinator(this, _stateMachine);

        if (uiManager != null)
        {
            uiManager.SpinRequested += _commandCoordinator.HandleSpinRequested;
            uiManager.WalkAwayRequested += _commandCoordinator.HandleWalkAwayRequested;
            uiManager.ReviveRequested += _commandCoordinator.HandleReviveRequested;
            uiManager.GiveUpRequested += _commandCoordinator.HandleGiveUpRequested;
        }
    }

    private void OnDestroy()
    {
        if (uiManager != null)
        {
            uiManager.SpinRequested -= _commandCoordinator.HandleSpinRequested;
            uiManager.WalkAwayRequested -= _commandCoordinator.HandleWalkAwayRequested;
            uiManager.ReviveRequested -= _commandCoordinator.HandleReviveRequested;
            uiManager.GiveUpRequested -= _commandCoordinator.HandleGiveUpRequested;
        }
    }

    /// <summary>
    /// Starts the game by entering the initial state.
    /// </summary>
    private void Start()
    {
        Debug.Log("[GameManager] Game is starting...");
        _stateMachine.ChangeState(new InitState());
    }

    #endregion

    #region State Transition API

    /// <summary>
    /// Handles the transition between different game states.
    /// Exits the current state and enters the new one.
    /// </summary>
    /// <param name="newState">The new state to transition to.</param>
    public void ChangeState(IGameState newState)
    {
        _stateMachine.ChangeState(newState);
    }

    #endregion

#if UNITY_EDITOR
    #region Editor Validation

    private void OnValidate()
    {
        if (uiManager == null)
        {
            Debug.LogWarning("[GameManager] UIManager reference is missing.", this);
        }

        if (wheelManager == null)
        {
            Debug.LogWarning("[GameManager] WheelManager reference is missing.", this);
        }

        if (zoneManager == null)
        {
            Debug.LogWarning("[GameManager] ZoneManager reference is missing.", this);
        }

        if (inventoryManager == null)
        {
            Debug.LogWarning("[GameManager] InventoryManager reference is missing.", this);
        }
    }

    #endregion
#endif
}
}