using UnityEngine;
using BloodLine.Core;
using BloodLine.Core.Configuration;
using BloodLine.Presentation;

namespace BloodLine.Main
{
    public class Bootstrapper : MonoBehaviour
    {
        private ServiceRegistry _registry;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
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
            
            Debug.Log("====== BOOTSTRAPPER INIT END ======");
        }
    }
}
