namespace BloodLine.Core.Simulation.State
{
    /// <summary>
    /// Contract for the central Game State Machine.
    /// Manages high-level execution context (Boot, Gameplay, Paused, etc).
    /// </summary>
    public interface IGameStateMachine
    {
        event System.Action<GameState> OnStateChanged;
        GameState CurrentState { get; }
        void ChangeState(GameState newState);
        void Update(float deltaTime);
    }
}
