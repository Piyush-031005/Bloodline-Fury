using UnityEngine;
using BloodLine.Core.Simulation;
using BloodLine.Modules.Character;
using BloodLine.Modules.Animation.State;

namespace BloodLine.Modules.Animation.Presentation
{
    /// <summary>
    /// Pure Unity presentation bridge. Reads the resolved AnimationState and passes it to an Animator.
    /// It NEVER executes gameplay logic or reads inputs directly.
    /// </summary>
    public class AnimationDriver : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        
        private IUpdateLoop _updateLoop;
        private PlayerPawn _playerPawn;
        private AnimationState _previousState;

        public void Inject(IUpdateLoop updateLoop, PlayerPawn playerPawn)
        {
            _updateLoop = updateLoop;
            _playerPawn = playerPawn;
            
            // Temporary auto-binding if not set in inspector
            if (_animator == null)
            {
                _animator = GetComponentInChildren<Animator>();
            }

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
            if (_playerPawn == null) return;

            AnimationState currentState = _playerPawn.CurrentState.AnimState;

            if (currentState != _previousState)
            {
                PlayAnimation(currentState);
                _previousState = currentState;
            }
        }

        private void PlayAnimation(AnimationState state)
        {
            // Milestone placeholder verification printout
            Debug.Log($"[AnimationDriver] Transitioned to visual state: {state}");

            if (_animator == null) return;

            // In a real implementation, we map the enum to Animator state hashes.
            // For now, we assume simple string matching or skip if Animator doesn't exist.
            // Example: _animator.CrossFade(state.ToString(), 0.1f);
        }
    }
}
