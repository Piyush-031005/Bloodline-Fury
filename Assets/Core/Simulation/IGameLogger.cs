using System;

namespace BloodLine.Core
{
    public enum LogLevel
    {
        Info,
        Warning,
        Error,
        Critical
    }

    /// <summary>
    /// Deterministic logger interface. 
    /// Ensures the simulation layer remains completely decoupled from UnityEngine's Debug.Log.
    /// </summary>
    public interface IGameLogger
    {
        void Log(string message, LogLevel level = LogLevel.Info);
        void LogFormat(LogLevel level, string format, params object[] args);
    }
}
