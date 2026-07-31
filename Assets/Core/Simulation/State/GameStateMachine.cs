using BloodLine.Presentation;

namespace BloodLine.Core.Simulation.State
{
    public class GameStateMachine : IGameStateMachine
    {
        public event System.Action<GameState> OnStateChanged;

        private readonly IGameLogger _logger;
        private GameState _currentState;

        public GameState CurrentState => _currentState;

        public GameStateMachine(IGameLogger logger)
        {
            _logger = logger;
            _currentState = GameState.Boot;
            _logger.Log($"[FSM] Game State Machine initialized in State: {_currentState}", LogLevel.Info);
        }

        public void ChangeState(GameState newState)
        {
            if (_currentState == newState) return;

            var oldState = _currentState;
            _currentState = newState;
            
            _logger.Log($"[FSM] State Changed: {oldState} -> {_currentState}", LogLevel.Info);
            OnStateChanged?.Invoke(_currentState);
        }

        public void Update(float deltaTime)
        {
            // Future logic: Trigger state-specific simulation ticks
        }
    }
}
