using UnityEditor;
using BloodLine.Main;

namespace BloodLine.Editor
{
    [InitializeOnLoad]
    public static class PlayModeBootstrapper
    {
        static PlayModeBootstrapper()
        {
            EditorApplication.playModeStateChanged += state =>
            {
                if (state == PlayModeStateChange.EnteredPlayMode)
                {
                    Bootstrapper.Init();
                }
            };
        }
    }
}
