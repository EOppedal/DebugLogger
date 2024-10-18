using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "DebugLogger", menuName = "Scrubs/DebugLogger")]
public class DebugLoggerSO : ScriptableObject {
    public string loggerName = "DebugLogger";
    public string logPrefix = "Unnamed";
    public bool enabled;

    [HideInInspector] public Object messageOrigin;

    public void Log(object message, [CanBeNull] Object origin = null) {
        if (enabled) Debug.Log($"[{logPrefix}] {message}", origin ?? messageOrigin);
    }

    public void LogWarning(object message, [CanBeNull] Object origin = null) {
        if (enabled) Debug.LogWarning($"[{logPrefix}] {message}", origin ?? messageOrigin);
    }

    public void LogError(object message, [CanBeNull] Object origin = null) {
        if (enabled) Debug.LogError($"[{logPrefix}] {message}", origin ?? messageOrigin);
    }
}
