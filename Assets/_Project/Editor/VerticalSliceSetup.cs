using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace BloodLine.Editor
{
    public static class VerticalSliceSetup
    {
        [MenuItem("BloodLine/Setup Vertical Slice Scenes")]
        public static void Setup()
        {
            // Ensure folder exists
            if (!AssetDatabase.IsValidFolder("Assets/_Project/Scenes"))
            {
                AssetDatabase.CreateFolder("Assets/_Project", "Scenes");
            }

            // Create Boot Scene
            var bootScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            string bootPath = "Assets/_Project/Scenes/Boot.unity";
            EditorSceneManager.SaveScene(bootScene, bootPath);

            // Create Simulation Scene
            var simScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            string simPath = "Assets/_Project/Scenes/Simulation.unity";

            // Create Ground
            var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Ground Plane";
            ground.transform.position = Vector3.zero;

            // Create Player Capsule
            var player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            player.name = "Temporary Player Capsule";
            player.transform.position = new Vector3(-2, 1, 0);

            // Create Dummy Cube
            var dummy = GameObject.CreatePrimitive(PrimitiveType.Cube);
            dummy.name = "Temporary Dummy Cube";
            dummy.transform.position = new Vector3(2, 0.5f, 0);

            // Setup Camera
            var mainCam = Camera.main;
            if (mainCam != null)
            {
                mainCam.transform.position = new Vector3(0, 3, -7);
                mainCam.transform.LookAt(Vector3.up * 1f);
                
                // Add HDRP Camera Data to fix "No cameras rendering"
                if (mainCam.GetComponent<UnityEngine.Rendering.HighDefinition.HDAdditionalCameraData>() == null)
                {
                    mainCam.gameObject.AddComponent<UnityEngine.Rendering.HighDefinition.HDAdditionalCameraData>();
                }
            }

            EditorSceneManager.SaveScene(simScene, simPath);

            // Add to Build Settings (Ensures scene loading works)
            EditorBuildSettings.scenes = new[]
            {
                new EditorBuildSettingsScene(bootPath, true),
                new EditorBuildSettingsScene(simPath, true)
            };

            Debug.Log("[VerticalSliceSetup] Created Boot and Simulation scenes, populated test environment, and added to Build Settings.");

            // Reload Boot scene to be ready for Play Mode
            EditorSceneManager.OpenScene(bootPath);
        }
    }
}
