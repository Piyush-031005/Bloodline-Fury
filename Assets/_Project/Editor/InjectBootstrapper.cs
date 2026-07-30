using UnityEditor;
using UnityEngine;
using BloodLine.Main;

namespace BloodLine.Editor
{
    [InitializeOnLoad]
    public class InjectBootstrapper
    {
        static InjectBootstrapper()
        {
            // Execute once on compile to ensure the scene has the Bootstrapper.
            EditorApplication.delayCall += () =>
            {
                if (GameObject.Find("[SYSTEM] Bootstrapper") == null && !EditorApplication.isPlayingOrWillChangePlaymode)
                {
                    var go = new GameObject("[SYSTEM] Bootstrapper");
                    go.AddComponent<Bootstrapper>();
                    Debug.Log("Successfully injected [SYSTEM] Bootstrapper into the scene!");
                }
            };
        }
    }
}
