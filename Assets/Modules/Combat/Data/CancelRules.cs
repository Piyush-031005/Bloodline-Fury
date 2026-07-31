using System;

namespace BloodLine.Modules.Combat.Data
{
    /// <summary>
    /// Flags defining which types of moves can cancel this move, 
    /// and what phase of the move is eligible for cancellation.
    /// </summary>
    [Flags]
    public enum CancelFlags
    {
        None = 0,
        LightAttack = 1 << 0,
        HeavyAttack = 1 << 1,
        Special = 1 << 2,
        Jump = 1 << 3,
        Dash = 1 << 4,
        All = ~0
    }

    [Serializable]
    public struct CancelRules
    {
        /// <summary>
        /// Frame at which cancellations become valid.
        /// </summary>
        public int StartFrame;
        
        /// <summary>
        /// Frame at which cancellations are no longer valid.
        /// </summary>
        public int EndFrame;

        /// <summary>
        /// What types of moves are allowed during this window.
        /// </summary>
        public CancelFlags AllowedFlags;

        /// <summary>
        /// Does this cancel only apply if the move physically connected (hit or block)?
        /// </summary>
        public bool RequiresContact;
    }
}
