using System;
using UnityEngine;

namespace BloodLine.Modules.Combat.Data
{
    [Serializable]
    public struct HitboxData
    {
        /// <summary>
        /// Local position relative to the character's root or specific bone ID.
        /// </summary>
        public Vector3 LocalOffset;
        
        /// <summary>
        /// Size of the collision box.
        /// </summary>
        public Vector3 Size;

        /// <summary>
        /// Damage inflicted if this hitbox connects.
        /// </summary>
        public int Damage;

        /// <summary>
        /// Frames of hitstop (freeze) applied to both characters on impact.
        /// </summary>
        public int HitstopFrames;

        /// <summary>
        /// Frames of stun applied to the victim.
        /// </summary>
        public int HitStunFrames;

        /// <summary>
        /// Mathematical pushback vector applied to the victim.
        /// </summary>
        public Vector3 Pushback;
    }
}
