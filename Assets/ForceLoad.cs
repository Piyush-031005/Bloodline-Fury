using UnityEngine;
using BloodLine.Main;

public static class ForceLoad
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Run()
    {
        Debug.Log("====== FORCE LOAD RUNNING ======");
        Bootstrapper.Init();
    }
}
