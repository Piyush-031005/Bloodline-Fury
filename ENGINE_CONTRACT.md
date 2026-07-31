# BloodLine Fury - Engine Contract

This document acts as the constitution for BloodLine Fury's Engine Architecture. It strictly defines the ownership, inputs, and outputs of every major system. 
**If a system deviates from this contract, it is an architectural leak and must be refactored.**

---

## 1. Simulation Layer (Pure Native C#)
These systems determine physical and logical reality. They execute deterministically every fixed tick. They contain ZERO Unity GameObjects.

### `CharacterSimulationCoordinator`
* **Owner:** `Bootstrapper` / `GameUpdateLoop`
* **Inputs:** `IInputService`
* **Outputs:** `CharacterState` (The final logical snapshot for a tick)
* **Responsibility:** Orchestration only. It calls the physics and combat engines in order. It contains no gameplay logic itself.

### `KinematicCharacterController`
* **Owner:** `CharacterSimulationCoordinator`
* **Inputs:** `CharacterState (n)`, `IInputService`, `FixedDeltaTime`
* **Outputs:** `CharacterState (n+1)` (Specifically Position, Velocity, Grounded status)
* **Responsibility:** Deterministic movement physics, jumping, and ground resolution.

### `CombatSimulationEngine`
* **Owner:** `CharacterSimulationCoordinator`
* **Inputs:** `CharacterState (n)`, `IInputService`, `MoveBank`
* **Outputs:** `CharacterState (n+1)` (Specifically CombatPhase, ActiveMoveID, CurrentMoveFrame, Hitstop)
* **Responsibility:** Deterministic evaluation of `MoveDefinitions` (Startup, Active, Recovery phases) and exposing active hitboxes.

### `AnimationStateResolver`
* **Owner:** `CharacterSimulationCoordinator`
* **Inputs:** `CharacterState` (Movement + Combat properties)
* **Outputs:** `AnimationState` (Enum stored on `CharacterState.AnimState`)
* **Responsibility:** Translating the raw mathematical reality into a high-level visual intent (e.g., Idle, Run, PunchStartup).

---

## 2. Presentation Layer (Unity MonoBehaviours)
These systems only OBSERVE reality. They never modify it. They are "dumb" bridges to Unity's renderer.

### `PlayerPawn`
* **Owner:** Unity Scene
* **Inputs:** `CharacterState`
* **Outputs:** `transform.position`
* **Responsibility:** Copies the simulated mathematical position to the visual Unity GameObject.

### `AnimationDriver`
* **Owner:** Unity Scene (`PlayerPawn` GameObject)
* **Inputs:** `CharacterState.AnimState`
* **Outputs:** `Animator.Play()`, `Animator.CrossFade()`
* **Responsibility:** Translates the `AnimationState` enum into string commands for the Unity `Animator`. It does not execute logic.

### `CameraDirectorPawn`
* **Owner:** Unity Scene (`Main Camera`)
* **Inputs:** `PlayerPawn.CurrentState`
* **Outputs:** `transform.position`, `transform.rotation`
* **Responsibility:** Applies Cinematography math (`ShotComposer`) to the Unity Camera based on the target's logical position.

### `CombatDebugLogger`
* **Owner:** Unity Scene (`PlayerPawn` GameObject)
* **Inputs:** `CharacterState`
* **Outputs:** `Debug.Log()`
* **Responsibility:** Prints deterministic combat execution states to the Unity console for architecture verification.
