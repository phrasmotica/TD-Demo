using System.Runtime.CompilerServices;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Util
{
    /// <summary>
    /// Logger that includes a class and calling method name in its messages.
    /// </summary>
    public class MethodLogger
    {
        /// <summary>
        /// The class name.
        /// </summary>
        private readonly string _className;

        public MethodLogger(string callerClassName)
        {
            _className = callerClassName;
        }

        public void Log(string message, [CallerMemberName] string callerMethodName = "")
        {
            Debug.Log($"{_className}.{callerMethodName}(): {message}");
        }

        public void LogWarning(string message, [CallerMemberName] string callerMethodName = "")
        {
            Debug.LogWarning($"{_className}.{callerMethodName}(): {message}");
        }

        public void LogError(string message, [CallerMemberName] string callerMethodName = "")
        {
            Debug.LogError($"{_className}.{callerMethodName}(): {message}");
        }
    }
}
