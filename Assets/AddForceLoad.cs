using UnityEditor;
using UnityEngine;
using BloodLine.Main;

#if UNITY_EDITOR
[InitializeOnLoad]
public class AddForceLoad
{
    static AddForceLoad()
    {
        EditorApplication.delayCall += () =>
        {
            if (Camera.main != null && Camera.main.GetComponent<ForceLoad>() == null)
            {
                Camera.main.gameObject.AddComponent<ForceLoad>();
                Debug.Log("Attached ForceLoad to Main Camera");
            }
        };
    }
}
#endif
