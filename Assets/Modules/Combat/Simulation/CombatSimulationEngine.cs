using UnityEngine;
using BloodLine.Core.Input;
using BloodLine.Modules.Character;
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
        public CharacterState Tick(CharacterState state, IInputService input, MoveBank moveBank)
        {
            // 1. Input Evaluation (if neutral)
            state = EvaluateInput(state, input, moveBank);

            // If a move is active, execute the pipeline
            if (state.Combat.CurrentPhase != CombatPhase.Neutral)
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

        private CharacterState EvaluateInput(CharacterState state, IInputService input, MoveBank moveBank)
        {
            // Only evaluate new attacks if Neutral (Cancel windows handled later)
            if (state.Combat.CurrentPhase == CombatPhase.Neutral)
            {
                if (input.GetAttackInput())
                {
                    // Start a hardcoded "Punch" for this milestone
                    if (moveBank.TryGetMove("Punch", out MoveDefinition move))
                    {
                        state.Combat.ActiveMoveID = "Punch";
                        state.Combat.CurrentMoveFrame = 0;
                        state.Combat.CurrentPhase = CombatPhase.Startup;
                    }
                }
            }
            return state;
        }

        private CharacterState AdvanceMoveTime(CharacterState state)
        {
            if (state.Combat.HitstopFramesRemaining > 0)
            {
                state.Combat.HitstopFramesRemaining--;
                return state; // Frozen, do not advance move
            }

            state.Combat.CurrentMoveFrame++;
            return state;
        }

        private CharacterState EvaluatePhase(CharacterState state, MoveBank moveBank)
        {
            if (!moveBank.TryGetMove(state.Combat.ActiveMoveID, out MoveDefinition move))
            {
                // Move doesn't exist, hard reset
                state.Combat = CombatState.Default();
                return state;
            }

            int currentFrame = state.Combat.CurrentMoveFrame;
            var frameData = move.FrameData;

            if (currentFrame > frameData.TotalFrames)
            {
                // Move is completely finished
                state.Combat = CombatState.Default();
            }
            else if (currentFrame > frameData.StartupFrames + frameData.ActiveFrames)
            {
                state.Combat.CurrentPhase = CombatPhase.Recovery;
            }
            else if (currentFrame > frameData.StartupFrames)
            {
                state.Combat.CurrentPhase = CombatPhase.Active;
            }
            else
            {
                state.Combat.CurrentPhase = CombatPhase.Startup;
            }

            return state;
        }
    }
}
