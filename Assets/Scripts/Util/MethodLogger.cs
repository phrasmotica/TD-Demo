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

        /// <summary>
        /// The method name.
        /// </summary>
        private readonly string MethodName;

        public MethodLogger(
            string callerClassName,
            [CallerMemberName] string callerMethodName = "")
        {
            ClassName = callerClassName;
            MethodName = callerMethodName;
        }

        public void Log(string message)
        {
            Debug.Log($"{ClassName}.{MethodName}(): {message}");
        }

        public void LogWarning(string message)
        {
            Debug.LogWarning($"{ClassName}.{MethodName}(): {message}");
        }

        public void LogError(string message)
        {
            Debug.LogError($"{ClassName}.{MethodName}(): {message}");
        }
    }
}
