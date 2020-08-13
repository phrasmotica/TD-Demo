using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.Scripts.Util
{
    public class MethodLogger : IDisposable
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

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
