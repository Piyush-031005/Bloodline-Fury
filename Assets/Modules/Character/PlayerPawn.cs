using UnityEngine;
using BloodLine.Core.Simulation;
using BloodLine.Core.Input;
using BloodLine.Modules.Combat.Simulation;
using BloodLine.Modules.Combat.State;

namespace BloodLine.Modules.Character
{
    public class PlayerPawn : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;

        private IUpdateLoop _updateLoop;
        private IInputService _input;
        private MoveBank _moveBank;
        private KinematicCharacterController _controller;
        private CombatSimulationEngine _combatEngine;
        private CharacterState _state;
        private float _fixedDeltaTime;

        public CharacterState CurrentState => _state;

        public void Inject(IUpdateLoop updateLoop, IInputService input, MoveBank moveBank, int targetTickRate)
        {
            _updateLoop = updateLoop;
            _input = input;
            _moveBank = moveBank;
            
            _controller = new KinematicCharacterController(moveSpeed);
            _combatEngine = new CombatSimulationEngine();
            _fixedDeltaTime = 1f / targetTickRate;

            // Initialize logical state from Unity's transform hierarchy (once)
            _state = new CharacterState
            {
                Position = transform.position,
                Velocity = Vector3.zero,
                IsGrounded = false,
                Combat = CombatState.Default()
            };

            _updateLoop.OnTick += HandleTick;
        }

        private void OnDestroy()
        {
            if (_updateLoop != null)
            {
                _updateLoop.OnTick -= HandleTick;
            }
        }

        private void HandleTick()
        {
            if (_controller != null && _input != null && _combatEngine != null && _moveBank != null)
            {
                // Core Gameplay Loop for Character:
                // 1. Evaluate Combat State (Attacks, Hitstun, Freezes)
                _state = _combatEngine.Tick(_state, _input, _moveBank);

                // 2. Evaluate Physics & Movement (Gravity, Jump, Walking)
                // Note: If hitstop is active, we should theoretically freeze physics too, but we handle that in future milestones.
                _state = _controller.Tick(_state, _input, _fixedDeltaTime);
                
                // 3. Sync visual transform to logical state
                transform.position = _state.Position;
            }
        }
    }
}
