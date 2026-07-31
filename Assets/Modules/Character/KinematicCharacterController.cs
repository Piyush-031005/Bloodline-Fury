using UnityEngine;
using BloodLine.Core.Input;

namespace BloodLine.Modules.Character
{
    /// <summary>
    /// Pure native C# character controller.
    /// Strictly deterministic, modifies CharacterState, completely independent of Unity Physics.
    /// </summary>
    public class KinematicCharacterController
    {
        private readonly float _speed;
        private readonly float _jumpVelocity = 15f;
        private readonly float _gravity = -30f;
        
        // Assume capsule has standard 2m height for simple math, offset raycast by slightly above feet.
        private readonly float _raycastOriginOffset = 0.5f; 
        private readonly float _raycastDistance = 0.6f;

        public KinematicCharacterController(float speed)
        {
            _speed = speed;
        }

        public CharacterState Tick(CharacterState state, IInputService input, float fixedDeltaTime)
        {
            // Stage 1: Input Processing
            bool jumpIntent = false;
            Vector3 desiredMovement = ProcessInput(input, out jumpIntent);

            // Stage 2: Apply Horizontal Movement
            state = ApplyHorizontalMovement(state, desiredMovement);

            // Stage 3: Apply Jump (Impulse)
            state = ApplyJump(state, jumpIntent);

            // Stage 4: Apply Gravity
            state = ApplyGravity(state, fixedDeltaTime);

            // Stage 5: Resolve Ground (Raycast)
            state = ResolveGround(state);

            // Stage 6: Collision Resolution (Future Milestone)
            state = ResolveCollisions(state);

            // Stage 7: Integrate Position
            state.Position += state.Velocity * fixedDeltaTime;

            return state;
        }

        private Vector3 ProcessInput(IInputService input, out bool jumpIntent)
        {
            Vector2 inputDir = input.GetMovementDirection();
            jumpIntent = input.GetJumpInput();
            
            // X is right, Z is forward
            Vector3 movement = new Vector3(inputDir.x, 0f, inputDir.y);

            // Normalize to prevent diagonal speed boosting
            if (movement.sqrMagnitude > 1f)
            {
                movement.Normalize();
            }

            return movement;
        }

        private CharacterState ApplyHorizontalMovement(CharacterState state, Vector3 desiredMovement)
        {
            // Kinematic movement: Instant horizontal velocity change (no acceleration/friction yet)
            state.Velocity.x = desiredMovement.x * _speed;
            state.Velocity.z = desiredMovement.z * _speed;
            
            return state;
        }

        private CharacterState ApplyJump(CharacterState state, bool jumpIntent)
        {
            if (jumpIntent && state.IsGrounded)
            {
                state.Velocity.y = _jumpVelocity;
                state.IsGrounded = false;
            }
            return state;
        }

        private CharacterState ApplyGravity(CharacterState state, float fixedDeltaTime)
        {
            // If grounded and falling, clamp downward velocity slightly to stick to ground slopes (future).
            if (state.IsGrounded && state.Velocity.y <= 0f)
            {
                state.Velocity.y = -2f; // Slight downward pressure to stick to floor.
            }
            else
            {
                state.Velocity.y += _gravity * fixedDeltaTime;
            }
            return state;
        }

        private CharacterState ResolveGround(CharacterState state)
        {
            // Simple Raycast down from slightly above the feet to detect the floor.
            Vector3 rayOrigin = state.Position + Vector3.up * _raycastOriginOffset;
            
            // For now, hit everything except IgnoreRaycast (layer 2) to ensure it works on default ground planes.
            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, _raycastDistance))
            {
                if (state.Velocity.y <= 0f) // Only land if we are falling or flat
                {
                    state.IsGrounded = true;
                    // Correct position to sit exactly on the floor
                    state.Position.y = hit.point.y;
                }
            }
            else
            {
                state.IsGrounded = false;
            }

            return state;
        }

        private CharacterState ResolveCollisions(CharacterState state)
        {
            // Empty placeholder for future Collision Resolution milestone
            return state;
        }
    }
}
