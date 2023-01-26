using UnityEngine;

namespace TDDemo.Assets.Scripts.Extensions
{
    /// <summary>
    /// Extension methods for Transform components.
    /// </summary>
    public static class TransformExtensions
    {
        /// <summary>
        /// Sets this transform's position to that of the mouse.
        /// </summary>
        public static void FollowMouse(this Transform t)
        {
            var mousePosition = Input.mousePosition;
            var worldPoint = Camera.main.ScreenToWorldPoint(mousePosition);
            t.position = new Vector3(worldPoint.x, worldPoint.y, t.position.z);
        }

        public static float GetDistanceToPosition(this Transform t, Vector3 v)
        {
            return (t.position - v).magnitude;
        }

        public static float GetDistanceToObject(this Transform t, GameObject o)
        {
            return t.GetDistanceToPosition(o.transform.position);
        }
    }
}
