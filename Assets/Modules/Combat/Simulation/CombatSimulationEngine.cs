using UnityEngine;
using BloodLine.Core.Input;
using BloodLine.Modules.Combat.Data;
using BloodLine.Modules.Combat.State;

namespace BloodLine.Modules.Combat.Simulation
{
    /// <summary>
    /// The deterministic pipeline that executes combat logic frame-by-frame.
    /// Pure C#, no MonoBehaviours.
    /// </summary>
    public class CombatSimulationEngine
    {
        public CombatState Tick(CombatState state, IInputService input, MoveBank moveBank)
        {
            // 1. Input Evaluation (if neutral)
            state = EvaluateInput(state, input, moveBank);

            // If a move is active, execute the pipeline
            if (state.CurrentPhase != CombatPhase.Neutral)
            {
                // 2. Move Advancement
                state = AdvanceMoveTime(state);

                // 3. Phase Evaluation
                state = EvaluatePhase(state, moveBank);

                // 4. Hitbox Exposure (Future: Collision Engine will read this)
                // For now, we just ensure the state is correctly tracked
            }

            return state;
        }

        private CombatState EvaluateInput(CombatState state, IInputService input, MoveBank moveBank)
        {
            // Only evaluate new attacks if Neutral (Cancel windows handled later)
            if (state.CurrentPhase == CombatPhase.Neutral)
            {
                if (input.GetAttackInput())
                {
                    // Start a hardcoded "Punch" for this milestone
                    if (moveBank.TryGetMove("Punch", out MoveDefinition move))
                    {
                        state.ActiveMoveID = "Punch";
                        state.CurrentMoveFrame = 0;
                        state.CurrentPhase = CombatPhase.Startup;
                    }
                }
            }
            return state;
        }

        private CombatState AdvanceMoveTime(CombatState state)
        {
            if (state.HitstopFramesRemaining > 0)
            {
                state.HitstopFramesRemaining--;
                return state; // Frozen, do not advance move
            }

            state.CurrentMoveFrame++;
            return state;
        }

        private CombatState EvaluatePhase(CombatState state, MoveBank moveBank)
        {
            if (!moveBank.TryGetMove(state.ActiveMoveID, out MoveDefinition move))
            {
                // Move doesn't exist, hard reset
                state = CombatState.Default();
                return state;
            }

            int currentFrame = state.CurrentMoveFrame;
            var frameData = move.FrameData;

            if (currentFrame > frameData.TotalFrames)
            {
                // Move is completely finished
                state = CombatState.Default();
            }
            else if (currentFrame > frameData.StartupFrames + frameData.ActiveFrames)
            {
                state.CurrentPhase = CombatPhase.Recovery;
            }
            else if (currentFrame > frameData.StartupFrames)
            {
                state.CurrentPhase = CombatPhase.Active;
            }
            else
            {
                state.CurrentPhase = CombatPhase.Startup;
            }

            return state;
        }
    }
}
