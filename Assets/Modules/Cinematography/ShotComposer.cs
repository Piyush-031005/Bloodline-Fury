using UnityEngine;

namespace BloodLine.Modules.Cinematography
{
    /// <summary>
    /// Pure mathematical C# system. 
    /// Translates directorial Camera Intents into absolute deterministic Camera States.
    /// Does NOT touch Unity Transforms or the Unity Camera.
    /// </summary>
    public class ShotComposer
    {
        // For today's milestone, we hardcode the Observe offset.
        // Future milestones will load this from the Theme Engine or Camera Profiles.
        private readonly Vector3 _observeOffset = new Vector3(0f, 4f, -8f);
        private readonly float _defaultFov = 60f;

        public CameraState Compose(CameraState currentState, CameraIntent intent, Vector3 primaryFocusPoint, float fixedDeltaTime)
        {
            switch (intent)
            {
                case CameraIntent.Observe:
                    return ProcessObserve(currentState, primaryFocusPoint, fixedDeltaTime);
                
                case CameraIntent.Engage:
                case CameraIntent.Pressure:
                case CameraIntent.Impact:
                case CameraIntent.Reveal:
                default:
                    // Future extension points. Fallback to Observe for now.
                    return ProcessObserve(currentState, primaryFocusPoint, fixedDeltaTime);
            }
        }

        private CameraState ProcessObserve(CameraState currentState, Vector3 focusPoint, float fixedDeltaTime)
        {
            // The camera acts as a true observer, maintaining a fixed tracking distance.
            // No interpolation is applied here as per the "no speculative engineering" rule.
            return new CameraState
            {
                Position = focusPoint + _observeOffset,
                LookAtTarget = focusPoint,
                FieldOfView = _defaultFov
            };
        }
    }
}
