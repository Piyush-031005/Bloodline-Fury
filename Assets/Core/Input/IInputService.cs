using UnityEngine;

namespace BloodLine.Core.Input
{
    public interface IInputService
    {
        Vector2 GetMovementDirection();
        bool GetJumpInput();
        bool GetAttackInput();
    }
}
