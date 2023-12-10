using System.Linq;
using UnityEngine;

namespace TDDemo
{
    public class Waypoints : MonoBehaviour
    {
        public static Transform[] Points;

        private void Awake()
        {
            // exclude the transform of this component
            Points = GetComponentsInChildren<Transform>().Skip(1).ToArray();
        }
    }
}
