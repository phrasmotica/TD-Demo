using UnityEngine;

// adapted from https://gist.github.com/Abban/42721b25cebba33c389a

namespace TDDemo.Assets.Scripts.Path
{
    public class Waypoint : MonoBehaviour
    {
        /// <summary>
        /// The number of seconds a follower must wait upon reaching this waypoint.
        /// </summary>
        public float WaitSeconds { get; set; }

        /// <summary>
        /// The speed a follower should adopt upon leaving this waypoint.
        /// </summary>
        public float SpeedOut { get; set; }
    }
}
