using UnityEngine;
using BloodLine.Core.Simulation;
using BloodLine.Core.Input;

namespace BloodLine.Modules.Character
{
    public class PlayerPawn : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;

        private IUpdateLoop _updateLoop;
        private IInputService _input;
        private KinematicCharacterController _controller;
        private CharacterState _state;
        private float _fixedDeltaTime;

        public CharacterState CurrentState => _state;

        public void Inject(IUpdateLoop updateLoop, IInputService input, int targetTickRate)
        {
            _updateLoop = updateLoop;
            _input = input;
            
            _controller = new KinematicCharacterController(moveSpeed);
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
            if (_controller != null && _input != null)
            {
                // Core Gameplay Loop for Character:
                // 1. Pass input and current state to deterministic controller
                // 2. Receive new updated state
                // 3. Sync visual transform to logical state
                _state = _controller.Tick(_state, _input, _fixedDeltaTime);
                transform.position = _state.Position;
            }
        }
    }
}
