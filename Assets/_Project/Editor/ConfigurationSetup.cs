using UnityEditor;
using UnityEngine;
using BloodLine.Core.Configuration;
using System.IO;

namespace BloodLine.Editor
{
    [InitializeOnLoad]
    public static class ConfigurationSetup
    {
        static ConfigurationSetup()
        {
            EditorApplication.delayCall += EnsureConfigurationExists;
        }

        private static void EnsureConfigurationExists()
        {
            string resourcesFolderPath = "Assets/Core/Configuration/Resources";
            
            // Ensure the folder exists
            if (!AssetDatabase.IsValidFolder(resourcesFolderPath))
            {
                // Create intermediate folders if necessary
                if (!AssetDatabase.IsValidFolder("Assets/Core"))
                    AssetDatabase.CreateFolder("Assets", "Core");
                if (!AssetDatabase.IsValidFolder("Assets/Core/Configuration"))
                    AssetDatabase.CreateFolder("Assets/Core", "Configuration");
                    
                AssetDatabase.CreateFolder("Assets/Core/Configuration", "Resources");
                AssetDatabase.Refresh();
            }

            string assetPath = $"{resourcesFolderPath}/GameConfiguration.asset";
            var existingConfig = AssetDatabase.LoadAssetAtPath<GameConfigurationAsset>(assetPath);
            
            if (existingConfig == null)
            {
                var newConfig = ScriptableObject.CreateInstance<GameConfigurationAsset>();
                AssetDatabase.CreateAsset(newConfig, assetPath);
                AssetDatabase.SaveAssets();
                Debug.Log($"[ConfigurationSetup] Created default GameConfiguration at {assetPath}");
            }
        }
    }
}
