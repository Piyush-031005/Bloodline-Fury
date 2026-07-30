using UnityEditor;
using UnityEngine;
using BloodLine.Main;

namespace BloodLine.Editor
{
    public static class SetupBootstrapper
    {
        [MenuItem("BloodLine/Setup Bootstrapper")]
        public static void Setup()
        {
            GameObject go = GameObject.Find("[SYSTEM] Bootstrapper");
            if (go == null)
            {
                go = new GameObject("[SYSTEM] Bootstrapper");
                go.AddComponent<Bootstrapper>();
                Debug.Log("[Setup] Created Bootstrapper");
            }
        }
    }
}
