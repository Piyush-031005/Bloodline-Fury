using UnityEngine;

namespace BloodLine.Core.Configuration
{
    /// <summary>
    /// Global game configuration interface.
    /// Exposes read-only properties for system configuration.
    /// </summary>
    public interface IGameConfiguration
    {
        int TargetTickRate { get; }
        bool ShowDebugHitboxes { get; }
        int RoundsToWin { get; }
    }
}
