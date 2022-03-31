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

        /// <summary>
        /// Returns the distance from this Transform to the given position.
        /// </summary>
        public static float GetDistanceToPosition(this Transform t, Vector3 v)
        {
            return (t.position - v).magnitude;
        }

        /// <summary>
        /// Returns the distance from this Transform to the given game object.
        /// </summary>
        public static float GetDistanceToObject(this Transform t, GameObject o)
        {
            return t.GetDistanceToPosition(o.transform.position);
        }

        public static int GetChildCountWithTag(this Transform t, string tag)
        {
            var count = 0;

            foreach (Transform child in t)
            {
                if (child.CompareTag(tag))
                {
                    count++;
                }
            }

            return count;
        }
    }
}
