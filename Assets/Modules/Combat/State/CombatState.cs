using System;

namespace BloodLine.Modules.Combat.State
{
    public enum CombatPhase
    {
        Neutral,
        Startup,
        Active,
        Recovery,
        Hitstun,
        Blockstun,
        Knockdown
    }

    /// <summary>
    /// Holds the deterministic combat state.
    /// This struct will be nested inside CharacterState to ensure 100% rollback compatibility.
    /// </summary>
    [Serializable]
    public struct CombatState
    {
        public CombatPhase CurrentPhase;
        
        /// <summary>
        /// ID of the move currently being executed (empty if Neutral).
        /// </summary>
        public string ActiveMoveID;

        /// <summary>
        /// How many frames the current move has been executing.
        /// </summary>
        public int CurrentMoveFrame;

        /// <summary>
        /// Hitstop freezes the character entirely for dramatic impact.
        /// </summary>
        public int HitstopFramesRemaining;

        /// <summary>
        /// How many frames of stun remain (used during Hitstun/Blockstun).
        /// </summary>
        public int StunFramesRemaining;

        public static CombatState Default()
        {
            return new CombatState
            {
                CurrentPhase = CombatPhase.Neutral,
                ActiveMoveID = string.Empty,
                CurrentMoveFrame = 0,
                HitstopFramesRemaining = 0,
                StunFramesRemaining = 0
            };
        }
    }
}
