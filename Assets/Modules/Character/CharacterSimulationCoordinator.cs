using BloodLine.Core.Input;
using BloodLine.Modules.Combat.Simulation;
using BloodLine.Modules.Character.Simulation;

namespace BloodLine.Modules.Character
{
    /// <summary>
    /// Pure C# orchestrator for a single character's simulation.
    /// It contains ZERO gameplay logic. It only routes data between isolated simulation engines.
    /// </summary>
    public class CharacterSimulationCoordinator
    {
        public CharacterState CurrentState { get; private set; }

        private readonly KinematicCharacterController _movementEngine;
        private readonly CombatSimulationEngine _combatEngine;
        private readonly MoveBank _moveBank;
        private readonly float _fixedDeltaTime;

        public CharacterSimulationCoordinator(CharacterState initialState, MoveBank moveBank, float moveSpeed, int targetTickRate)
        {
            CurrentState = initialState;
            _moveBank = moveBank;
            _fixedDeltaTime = 1f / targetTickRate;

            _movementEngine = new KinematicCharacterController(moveSpeed);
            _combatEngine = new CombatSimulationEngine();
        }

        public void Tick(IInputService input)
        {
            CharacterState nextState = CurrentState;

            // 1. Combat Engine (Pure Logic)
            nextState.Combat = _combatEngine.Tick(nextState.Combat, input, _moveBank);

            // 2. Movement Engine (Pure Logic)
            nextState = _movementEngine.Tick(nextState, input, _fixedDeltaTime);

            // 3. Resolve Animation State (Pure Logic)
            nextState.AnimState = AnimationStateResolver.Resolve(nextState);

            // 4. Finalize State
            CurrentState = nextState;
        }
    }
}
