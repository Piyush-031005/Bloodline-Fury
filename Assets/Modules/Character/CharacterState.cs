using UnityEngine;
using BloodLine.Modules.Combat.State;

namespace BloodLine.Modules.Character
{
    /// <summary>
    /// The single source of truth for the character's logical state in the deterministic simulation.
    /// No gameplay system may modify Unity Transform directly. Gameplay updates this state.
    /// </summary>
    public struct CharacterState
    {
        public Vector3 Position;
        public Vector3 Velocity;
        public bool IsGrounded;
        public CombatState Combat;
    }
}
