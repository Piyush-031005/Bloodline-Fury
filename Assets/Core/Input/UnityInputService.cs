using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BloodLine.Core.Input
{
    public class UnityInputService : IInputService, IDisposable
    {
        private InputAction _moveAction;
        private InputAction _jumpAction;

        public UnityInputService()
        {
            _moveAction = new InputAction(name: "Move", type: InputActionType.Value, expectedControlType: "Vector2");
            _moveAction.AddCompositeBinding("2DVector(mode=2)") // Mode 2 = analog
                .With("Up", "<Keyboard>/w")
                .With("Down", "<Keyboard>/s")
                .With("Left", "<Keyboard>/a")
                .With("Right", "<Keyboard>/d");

            _jumpAction = new InputAction(name: "Jump", type: InputActionType.Button);
            _jumpAction.AddBinding("<Keyboard>/space");
            _jumpAction.AddBinding("<Gamepad>/buttonSouth");

            _moveAction.Enable();
            _jumpAction.Enable();
        }

        public void Dispose()
        {
            _moveAction?.Disable();
            _moveAction?.Dispose();
            
            _jumpAction?.Disable();
            _jumpAction?.Dispose();
        }

        public Vector2 GetMovementDirection()
        {
            return _moveAction.ReadValue<Vector2>();
        }

        public bool GetJumpInput()
        {
            // Returns true only on the frame the button is pressed (Phase 1 simplicity)
            return _jumpAction.triggered;
        }
    }
}
