using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.Scripts.Util
{
    /// <summary>
    /// Logger that includes a class and calling method name in its messages.
    /// </summary>
    public class MethodLogger
    {
        /// <summary>
        /// The class name.
        /// </summary>
        private readonly string ClassName;

        public MethodLogger(string callerClassName)
        {
            ClassName = callerClassName;
        }

        public void Log(string message, [CallerMemberName] string callerMethodName = "")
        {
            Debug.Log($"{ClassName}.{callerMethodName}(): {message}");
        }

        public void LogWarning(string message, [CallerMemberName] string callerMethodName = "")
        {
            Debug.LogWarning($"{ClassName}.{callerMethodName}(): {message}");
        }

        public void LogError(string message, [CallerMemberName] string callerMethodName = "")
        {
            Debug.LogError($"{ClassName}.{callerMethodName}(): {message}");
        }
    }
}
