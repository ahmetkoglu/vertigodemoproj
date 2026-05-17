using UnityEngine;

public class GameManager : MonoBehaviour, IGameContext
{
    public static GameManager Instance { get; private set; }

    [Header("Sub-Managers")]
    [SerializeField] private UIManager uiManager;
    [SerializeField] private WheelManager wheelManager;
    [SerializeField] private ZoneManager zoneManager;
    [SerializeField] private InventoryManager inventoryManager;

    // IGameContext Implementations
    public UIManager UI => uiManager;
    public WheelManager Wheel => wheelManager;
    public ZoneManager Zone => zoneManager;
    public InventoryManager Inventory => inventoryManager;

    private IGameState _currentState;

    /// <summary>
    /// Initializes the Singleton instance. Ensures only one GameManager exists in the scene.
    /// </summary>
    private void Awake()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        if (Instance == null) 
        {
            Instance = this;
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Starts the game by entering the initial state.
    /// </summary>
    private void Start()
    {
        Debug.Log("[GameManager] Game is starting...");
        ChangeState(new InitState());
    }

    /// <summary>
    /// Handles the transition between different game states.
    /// Exits the current state and enters the new one.
    /// </summary>
    /// <param name="newState">The new state to transition to.</param>
    public void ChangeState(IGameState newState)
    {
        if (_currentState != null)
        {
            _currentState.ExitState(this);
            Debug.Log($"[GameManager] Exited State: {_currentState.GetType().Name}");
        }

        _currentState = newState;
        
        Debug.Log($"[GameManager] Entered State: {_currentState.GetType().Name}");
        _currentState.EnterState(this);
    }

    /// <summary>
    /// Triggered when the player clicks the 'Spin' button.
    /// Only works if the current state is IdleState.
    /// </summary>
    public void OnSpinButtonPressed()
    {
        if (_currentState is IdleState) 
        {
            ChangeState(new SpinningState());
        }
    }

    /// <summary>
    /// Triggered when the player clicks the 'Walk Away' button to claim rewards.
    /// </summary>
    public void OnWalkAwayButtonPressed()
    {
        if (_currentState is IdleState)
        {
            Debug.Log("[GameManager] Walk Away triggered!");
            ChangeState(new ClaimState());
        }
    }

    /// <summary>
    /// Triggered when the player chooses to revive after hitting a bomb.
    /// Bypasses the bomb and advances to the next zone.
    /// </summary>
    public void OnReviveButtonPressed()
    {
        if (_currentState is GameOverState)
        {
            Debug.Log("[GameManager] Player revived! Bomb avoided.");
            Zone.IncreaseLevel();
            ChangeState(new InitState());
        }
    }

    /// <summary>
    /// Triggered when the player gives up after hitting a bomb.
    /// Clears inventory, resets progress, and restarts the game loop.
    /// </summary>
    public void OnGiveUpButtonPressed()
    {
        if (_currentState is GameOverState)
        {
            Debug.Log("[GameManager] Player gave up. All progress is lost.");
            Inventory.ClearInventory();
            UI.ClearInventoryUI();
            Zone.ResetLevel();
            ChangeState(new InitState());
        }
    }
}