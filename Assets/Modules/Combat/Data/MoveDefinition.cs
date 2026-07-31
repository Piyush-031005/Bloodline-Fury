using System.Collections.Generic;
using UnityEngine;
using BloodLine.Modules.Cinematography;

namespace BloodLine.Modules.Combat.Data
{
    /// <summary>
    /// The fundamental data block that defines any combat action (Punch, Kick, Spell, Dodge).
    /// Authored by designers, parsed into structs at runtime for GC-free execution.
    /// </summary>
    [CreateAssetMenu(fileName = "New Move", menuName = "BloodLine/Combat/Move Definition")]
    public class MoveDefinition : ScriptableObject
    {
        [Header("Identity")]
        public string MoveID;
        public CancelFlags MoveType;

        [Header("Timeline")]
        public FrameData FrameData;

        [Header("Hitboxes (Evaluated during Active frames)")]
        public List<HitboxData> Hitboxes = new List<HitboxData>();

        [Header("Cancel Windows")]
        public List<CancelRules> CancelRules = new List<CancelRules>();

        [Header("Physics Overrides (Per Frame)")]
        [Tooltip("If true, overrides horizontal movement during active frames.")]
        public bool OverrideVelocity;
        public Vector3 VelocityOverride;

        [Header("Presentation (Themes & Intents)")]
        public CameraIntent CameraIntent = CameraIntent.Observe;
        
        [Tooltip("String ID mapped by the Theme Engine to an actual VFX prefab.")]
        public string VfxIntentID;
        
        [Tooltip("String ID mapped by the Theme Engine to an actual Audio clip.")]
        public string AudioIntentID;
        
        [Tooltip("The Animation State name to play in the Animator.")]
        public string AnimationStateName;
    }
}
