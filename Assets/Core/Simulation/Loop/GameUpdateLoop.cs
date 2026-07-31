using BloodLine.Presentation;
using BloodLine.Core.Simulation.State;

namespace BloodLine.Core.Simulation
{
    public class GameUpdateLoop : IUpdateLoop
    {
        public event System.Action OnTick;
        
        private readonly IGameLogger _logger;
        private readonly IGameStateMachine _stateMachine;
        
        private int _targetTickRate;
        private float _fixedDeltaTime;
        private float _accumulator;
        private int _tickCount;

        public GameUpdateLoop(IGameLogger logger, IGameStateMachine stateMachine)
        {
            _logger = logger;
            _stateMachine = stateMachine;
        }

        public void Initialize(int targetTickRate)
        {
            _targetTickRate = targetTickRate;
            _fixedDeltaTime = 1f / _targetTickRate;
            
            _logger.Log($"Update Loop initialized. TickRate: {_targetTickRate}, FixedDeltaTime: {_fixedDeltaTime:F4}s", LogLevel.Info);
        }

        public void Update(float deltaTime)
        {
            _stateMachine.Update(deltaTime);

            // Cap delta time to prevent spiral of death during heavy lag spikes
            if (deltaTime > 0.25f)
            {
                deltaTime = 0.25f;
            }

            _accumulator += deltaTime;

            // Consume accumulated time in fixed discrete steps
            while (_accumulator >= _fixedDeltaTime)
            {
                Tick();
                _accumulator -= _fixedDeltaTime;
            }
        }

        private void Tick()
        {
            _tickCount++;
            OnTick?.Invoke();
            // Future logic: Push tick event to TimeSystem or GameStateMachine
        }
    }
}
