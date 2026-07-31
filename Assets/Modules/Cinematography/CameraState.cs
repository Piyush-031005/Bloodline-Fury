using UnityEngine;

namespace BloodLine.Modules.Cinematography
{
    /// <summary>
    /// The single source of truth for the camera's logical state in the deterministic simulation.
    /// Represents the absolute mathematical state of the lens.
    /// </summary>
    public struct CameraState
    {
        public Vector3 Position;
        public Vector3 LookAtTarget;
        public float FieldOfView;
    }
}
