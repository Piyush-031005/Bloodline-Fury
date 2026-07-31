using UnityEngine;
using BloodLine.Core.Simulation;
using BloodLine.Modules.Character;

namespace BloodLine.Modules.Cinematography
{
    /// <summary>
    /// The presentation bridge for the Cinematography Engine.
    /// Strictly translates mathematical CameraState into Unity Camera transforms.
    /// </summary>
    public class CameraDirectorPawn : MonoBehaviour
    {
        private IUpdateLoop _updateLoop;
        private PlayerPawn _targetPlayer;
        
        private ShotComposer _composer;
        private CameraState _state;
        private CameraIntent _currentIntent;
        private float _fixedDeltaTime;

        private Camera _unityCamera;

        public void Inject(IUpdateLoop updateLoop, PlayerPawn targetPlayer, int targetTickRate)
        {
            _updateLoop = updateLoop;
            _targetPlayer = targetPlayer;
            _fixedDeltaTime = 1f / targetTickRate;

            _composer = new ShotComposer();
            _currentIntent = CameraIntent.Observe; // Default intent
            _unityCamera = GetComponent<Camera>();

            // Initialize logical state
            _state = new CameraState
            {
                Position = transform.position,
                LookAtTarget = transform.position + transform.forward,
                FieldOfView = _unityCamera != null ? _unityCamera.fieldOfView : 60f
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
            if (_composer == null || _targetPlayer == null || _unityCamera == null) return;

            // 1. Gameplay -> Intent -> Composer -> Camera State
            _state = _composer.Compose(_state, _currentIntent, _targetPlayer.CurrentState.Position, _fixedDeltaTime);

            // 2. Presentation Bridge: Apply State
            transform.position = _state.Position;
            transform.LookAt(_state.LookAtTarget);
            _unityCamera.fieldOfView = _state.FieldOfView;
        }
    }
}
