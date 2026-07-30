using UnityEngine;
using BloodLine.Core;

namespace BloodLine.Presentation
{
    /// <summary>
    /// Unity-specific implementation of the IGameLogger.
    /// Routes pure simulation logs directly to the Unity Console.
    /// </summary>
    public class UnityGameLogger : IGameLogger
    {
        public void Log(string message, LogLevel level = LogLevel.Info)
        {
            switch (level)
            {
                case LogLevel.Info:
                    Debug.Log($"[BloodLine] {message}");
                    break;
                case LogLevel.Warning:
                    Debug.LogWarning($"[BloodLine] {message}");
                    break;
                case LogLevel.Error:
                case LogLevel.Critical:
                    Debug.LogError($"[BloodLine] {message}");
                    break;
            }
        }

        public void LogFormat(LogLevel level, string format, params object[] args)
        {
            Log(string.Format(format, args), level);
        }
    }
}
