namespace BloodLine.Modules.Cinematography
{
    /// <summary>
    /// Represents the directorial intent for the combat cinematography engine.
    /// The camera responds to these intents rather than blindly following a target.
    /// </summary>
    public enum CameraIntent
    {
        /// <summary>
        /// Standard framing. Maintains an overview of the fighters.
        /// </summary>
        Observe,
        
        /// <summary>
        /// Fighters are closing in. Tightens the frame to focus on combat engagement.
        /// </summary>
        Engage,
        
        /// <summary>
        /// One fighter is cornered. Emphasizes the power dynamic and claustrophobia.
        /// </summary>
        Pressure,
        
        /// <summary>
        /// A heavy hit connects. Used for freeze frames, zooms, and dramatic impact framing.
        /// </summary>
        Impact,
        
        /// <summary>
        /// The fight concludes or a cinematic sequence begins. Reorients to highlight the victor.
        /// </summary>
        Reveal
    }
}
