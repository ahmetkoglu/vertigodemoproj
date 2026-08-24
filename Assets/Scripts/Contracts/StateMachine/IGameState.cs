namespace WheelGame.Contracts.StateMachine
{
    /// <summary>
    /// Contract for all game states in the State Machine.
    /// Defines the lifecycle methods that occur when entering and exiting a state.
    /// </summary>
    public interface IGameState
    {
        /// <summary>
        /// Called automatically when the state machine transitions into this state.
        /// </summary>
        /// <param name="context">The central game context providing access to services.</param>
        void EnterState(IGameContext context);

        /// <summary>
        /// Called automatically when the state machine transitions out of this state.
        /// </summary>
        /// <param name="context">The central game context providing access to services.</param>
        void ExitState(IGameContext context);
    }
}