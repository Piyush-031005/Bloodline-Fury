using UnityEngine;
using UnityEngine.SceneManagement;
using BloodLine.Core;
using BloodLine.Core.Simulation;
using BloodLine.Core.Simulation.State;
using BloodLine.Core.Configuration;
using BloodLine.Core.Input;
using BloodLine.Presentation;
using BloodLine.Modules.Character;
using BloodLine.Modules.Cinematography;

namespace BloodLine.Main
{
    public class Bootstrapper : MonoBehaviour
    {
        private ServiceRegistry _registry;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Init()
        {
            // Explicit Unity debug to guarantee it shows up in raw logs
            Debug.Log("====== BOOTSTRAPPER INIT START ======");

            var registry = new ServiceRegistry();

            // 1. Load and Register Configuration
            var config = Resources.Load<GameConfigurationAsset>("GameConfiguration");
            if (config == null)
            {
                Debug.LogWarning("[BloodLine] GameConfiguration not found in Resources. Using fallback defaults.");
                config = ScriptableObject.CreateInstance<GameConfigurationAsset>();
            }
            registry.Register<IGameConfiguration>(config);

            // 2. Register Logger
            registry.Register<IGameLogger>(new UnityGameLogger());
            
            // 3. Verify Logger
            var logger = registry.Get<IGameLogger>();
            logger.Log($"BloodLine Fury: Core Architecture Bootstrapped Successfully. Target Tick Rate: {registry.Get<IGameConfiguration>().TargetTickRate}", LogLevel.Info);
            
            // 4. Initialize Game State Machine
            var stateMachine = new GameStateMachine(logger);
            registry.Register<IGameStateMachine>(stateMachine);

            // 5. Initialize Update Loop
            var updateLoop = new GameUpdateLoop(logger, stateMachine);
            registry.Register<IUpdateLoop>(updateLoop);
            updateLoop.Initialize(config.TargetTickRate);

            // 6. Initialize Input Service
            var inputService = new UnityInputService();
            registry.Register<IInputService>(inputService);

            // 7. Spawn Update Loop Runner
            var loopRunnerGO = new GameObject("[SYSTEM] UpdateLoopRunner");
            Object.DontDestroyOnLoad(loopRunnerGO);
            var runner = loopRunnerGO.AddComponent<UpdateLoopRunner>();
            runner.Inject(updateLoop);

            // 7. Wire Scene Loading to State Machine
            stateMachine.OnStateChanged += (state) =>
            {
                if (state == GameState.Gameplay)
                {
                    // Initialize Player Pawn
                    PlayerPawn playerPawn = null;
                    var playerGO = GameObject.Find("Temporary Player Capsule");
                    if (playerGO != null)
                    {
                        playerPawn = playerGO.GetComponent<PlayerPawn>();
                        if (playerPawn == null) playerPawn = playerGO.AddComponent<PlayerPawn>();
                        
                        playerPawn.Inject(registry.Get<IUpdateLoop>(), registry.Get<IInputService>(), registry.Get<IGameConfiguration>().TargetTickRate);
                        logger.Log("[Bootstrapper] PlayerPawn initialized successfully.", LogLevel.Info);
                    }
                    else
                    {
                        logger.Log("[Bootstrapper] Temporary Player Capsule not found in scene.", LogLevel.Warning);
                    }

                    // Initialize Cinematography Engine
                    var mainCamera = Camera.main;
                    if (mainCamera != null && playerPawn != null)
                    {
                        var director = mainCamera.gameObject.GetComponent<CameraDirectorPawn>();
                        if (director == null) director = mainCamera.gameObject.AddComponent<CameraDirectorPawn>();
                        
                        director.Inject(registry.Get<IUpdateLoop>(), playerPawn, registry.Get<IGameConfiguration>().TargetTickRate);
                        logger.Log("[Bootstrapper] CameraDirectorPawn initialized successfully.", LogLevel.Info);
                    }
                    else
                    {
                        logger.Log("[Bootstrapper] Main Camera or PlayerPawn not found. Cannot initialize Cinematography Engine.", LogLevel.Warning);
                    }
                }
                else if (state == GameState.Loading)
                {
                    logger.Log("[Bootstrapper] Triggering Scene Load for Simulation...", LogLevel.Info);

                    // --- STABILITY FIX: Prevent duplicate loading if Simulation is already open in Editor ---
                    var existingScene = SceneManager.GetSceneByName("Simulation");
                    if (existingScene.IsValid())
                    {
                        logger.Log("[Bootstrapper] Simulation scene is already loaded. Skipping async load.", LogLevel.Info);
                        SceneManager.SetActiveScene(existingScene);
                        
                        var existingBootCam = GameObject.Find("Boot Camera");
                        if (existingBootCam != null) Object.Destroy(existingBootCam);
                        
                        stateMachine.ChangeState(GameState.Gameplay);
                        return;
                    }
                    // -----------------------------------------------------------------------------------------

                    var asyncOp = SceneManager.LoadSceneAsync("Simulation", LoadSceneMode.Additive);
                    if (asyncOp != null)
                    {
                        asyncOp.completed += _ =>
                        {
                            logger.Log("[Bootstrapper] Simulation Scene loaded successfully.", LogLevel.Info);

                            // Make Simulation the active scene so new objects prioritize it
                            var simScene = SceneManager.GetSceneByName("Simulation");
                            if (simScene.IsValid() && simScene.isLoaded)
                            {
                                SceneManager.SetActiveScene(simScene);
                            }

                            // Destroy Boot Camera if it exists to allow Simulation Camera to take over
                            var bootCam = GameObject.Find("Boot Camera");
                            if (bootCam != null)
                            {
                                Object.Destroy(bootCam);
                            }

                            stateMachine.ChangeState(GameState.Gameplay);
                        };
                    }
                    else
                    {
                        logger.Log("[Bootstrapper] Failed to trigger Simulation scene load. Check Build Settings.", LogLevel.Error);
                    }
                }
            };

            // 8. Test State Transition to trigger the loading chain
            stateMachine.ChangeState(GameState.Loading);

            Debug.Log("====== BOOTSTRAPPER INIT END ======");
        }
    }
}
