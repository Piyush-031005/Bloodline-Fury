using UnityEditor;
using UnityEngine;
using BloodLine.Main;

namespace BloodLine.Editor
{
    [InitializeOnLoad]
    public static class AutoBootstrapper
    {
        static AutoBootstrapper()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                GameObject go = new GameObject("[SYSTEM] Bootstrapper");
                go.AddComponent<Bootstrapper>();
            }
        }
    }
}
