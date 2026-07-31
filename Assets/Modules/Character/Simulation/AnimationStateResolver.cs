using BloodLine.Modules.Character;
using BloodLine.Modules.Combat.State;
using BloodLine.Modules.Animation.State;

namespace BloodLine.Modules.Character.Simulation
{
    /// <summary>
    /// Pure C# stateless function that determines the Animation Intent based solely on physical and combat reality.
    /// It NEVER modifies physics or combat state.
    /// Owned by the Character assembly.
    /// </summary>
    public static class AnimationStateResolver
    {
        public static AnimationState Resolve(CharacterState state)
        {
            // 1. Highest Priority: Combat Phase Overrides
            if (state.Combat.CurrentPhase != CombatPhase.Neutral)
            {
                switch (state.Combat.CurrentPhase)
                {
                    case CombatPhase.Startup: return AnimationState.AttackStartup;
                    case CombatPhase.Active: return AnimationState.AttackActive;
                    case CombatPhase.Recovery: return AnimationState.AttackRecovery;
                    case CombatPhase.Hitstun: return AnimationState.Hitstun;
                    case CombatPhase.Blockstun: return AnimationState.Blockstun;
                    case CombatPhase.Knockdown: return AnimationState.Knockdown;
                }
            }

            // 2. Medium Priority: Airborne Physics
            if (!state.IsGrounded)
            {
                if (state.Velocity.y > 0)
                {
                    return AnimationState.Jump;
                }
                return AnimationState.Fall;
            }

            // 3. Lowest Priority: Ground Physics
            if (state.Velocity.x != 0 || state.Velocity.z != 0)
            {
                return AnimationState.Walk; // Future: Check velocity magnitude to return Run vs Walk
            }

            return AnimationState.Idle;
        }
    }
}
