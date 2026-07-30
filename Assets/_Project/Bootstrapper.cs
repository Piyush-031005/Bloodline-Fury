using UnityEngine;
using BloodLine.Core;
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
            registry.Register<IGameLogger>(new UnityGameLogger());
            
            var logger = registry.Get<IGameLogger>();
            logger.Log("BloodLine Fury: Core Architecture Bootstrapped Successfully.", LogLevel.Info);
            
            Debug.Log("====== BOOTSTRAPPER INIT END ======");
        }
    }
}
