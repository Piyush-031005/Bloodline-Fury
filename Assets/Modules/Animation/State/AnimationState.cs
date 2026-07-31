using System;

namespace BloodLine.Modules.Animation.State
{
    /// <summary>
    /// Pure logical animation intents output by the AnimationStateResolver.
    /// This has absolutely zero knowledge of Unity's Animator or Clips.
    /// </summary>
    [Serializable]
    public enum AnimationState
    {
        Idle,
        Walk,
        Jump,
        Fall,
        AttackStartup,
        AttackActive,
        AttackRecovery,
        Hitstun,
        Blockstun,
        Knockdown
    }
}
