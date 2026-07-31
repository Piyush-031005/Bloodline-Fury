using UnityEngine;

namespace BloodLine.Core.Configuration
{
    /// <summary>
    /// Data-driven configuration asset. 
    /// Should be instantiated as a ScriptableObject in the Resources folder.
    /// </summary>
    [CreateAssetMenu(fileName = "GameConfiguration", menuName = "BloodLine/Configuration/Game Configuration")]
    public class GameConfigurationAsset : ScriptableObject, IGameConfiguration
    {
        [Header("Simulation Settings")]
        [SerializeField, Tooltip("Target fixed tick rate for the simulation layer.")]
        private int _targetTickRate = 60;

        [Header("Debug Settings")]
        [SerializeField, Tooltip("Globally toggle debug hitboxes rendering.")]
        private bool _showDebugHitboxes = false;

        [Header("Game Rules")]
        [SerializeField, Tooltip("Number of rounds required to win the match.")]
        private int _roundsToWin = 2;

        public int TargetTickRate => _targetTickRate;
        public bool ShowDebugHitboxes => _showDebugHitboxes;
        public int RoundsToWin => _roundsToWin;
    }
}
