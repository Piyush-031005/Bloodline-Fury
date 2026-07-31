using System;
using UnityEngine;
using BloodLine.Core.Simulation;
using BloodLine.Modules.Combat.State;

namespace BloodLine.Modules.Combat.Presentation
{
    /// <summary>
    /// Temporary presentation bridge to visualize the Combat Simulation Engine in the console.
    /// Used purely to verify the deterministic pipeline without an animation rig.
    /// </summary>
    public class CombatDebugLogger : MonoBehaviour
    {
        private IUpdateLoop _updateLoop;
        private Func<CombatState> _getCombatState;
        private CombatState _previousState;

        public void Inject(IUpdateLoop updateLoop, Func<CombatState> getCombatState)
        {
            _updateLoop = updateLoop;
            _getCombatState = getCombatState;
            _previousState = CombatState.Default();

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
            if (_getCombatState == null) return;

            var currentState = _getCombatState();

            // Only log if something interesting is happening to prevent console flooding
            if (currentState.CurrentPhase != CombatPhase.Neutral || _previousState.CurrentPhase != CombatPhase.Neutral)
            {
                // Only log when frame changes or phase changes to keep it readable
                if (currentState.CurrentMoveFrame != _previousState.CurrentMoveFrame || 
                    currentState.CurrentPhase != _previousState.CurrentPhase)
                {
                    string phaseStr = currentState.CurrentPhase.ToString();
                    if (currentState.CurrentPhase == CombatPhase.Active && _previousState.CurrentPhase != CombatPhase.Active)
                    {
                        phaseStr += " -> Hitbox Exposed!";
                    }

                    if (currentState.CurrentPhase == CombatPhase.Neutral)
                    {
                        Debug.Log($"[CombatEngine] [Frame {currentState.CurrentMoveFrame}] Neutral");
                    }
                    else
                    {
                        Debug.Log($"[CombatEngine] [Frame {currentState.CurrentMoveFrame}] {phaseStr} ({currentState.ActiveMoveID})");
                    }
                }
            }

            _previousState = currentState;
        }
    }
}
