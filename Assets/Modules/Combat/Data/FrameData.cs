using System;

namespace BloodLine.Modules.Combat.Data
{
    /// <summary>
    /// Represents the pure timeline of a move in deterministic simulation frames.
    /// Example: A 30-frame move could be 10 frames Startup, 5 frames Active, 15 frames Recovery.
    /// </summary>
    [Serializable]
    public struct FrameData
    {
        public int StartupFrames;
        public int ActiveFrames;
        public int RecoveryFrames;

        public int TotalFrames => StartupFrames + ActiveFrames + RecoveryFrames;

        /// <summary>
        /// Returns true if the given frame is within the active hit window.
        /// </summary>
        public bool IsActiveFrame(int currentFrame)
        {
            return currentFrame >= StartupFrames && currentFrame < (StartupFrames + ActiveFrames);
        }
    }
}
