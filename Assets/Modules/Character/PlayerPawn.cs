using UnityEngine;
using BloodLine.Core.Simulation;
using BloodLine.Modules.Combat.State;

namespace BloodLine.Modules.Character
{
    /// <summary>
    /// Pure Presentation Bridge. It does NOT own any simulation logic.
    /// It receives the latest CharacterState and updates the visual representation.
    /// </summary>
    public class PlayerPawn : MonoBehaviour
    {
        private IUpdateLoop _updateLoop;
        private CharacterSimulationCoordinator _coordinator;
        private CharacterState _latestState;

        public CharacterState CurrentState => _latestState;

        public void Inject(IUpdateLoop updateLoop, CharacterSimulationCoordinator coordinator)
        {
            _updateLoop = updateLoop;
            _coordinator = coordinator;
            _latestState = coordinator.CurrentState;

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
            if (_coordinator != null)
            {
                // Retrieve the purely simulated state
                _latestState = _coordinator.CurrentState;
                
                // Sync visual transform to logical state
                transform.position = _latestState.Position;
            }
        }
    }
}
