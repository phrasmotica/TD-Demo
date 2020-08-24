using System.Linq;
using Assets.Scripts.Path;
using Assets.Scripts.Util;
using UnityEngine;

// adapted from https://gist.github.com/Abban/42721b25cebba33c389a

namespace Assets.Scripts.Enemies
{
    /// <summary>
    /// Script for making an object move in straight lines along a path of waypoints.
    /// </summary>
    public class WaypointFollower : BaseBehaviour
    {
        /// <summary>
        /// The waypoints to follow.
        /// </summary>
        private Waypoint[] waypoints;

        /// <summary>
        /// The current destination waypoint.
        /// </summary>
        private Waypoint CurrentWaypoint => (waypoints?.Any() ?? false) ? waypoints[currentIndex] : null;

        /// <summary>
        /// The speed with which to move towards the waypoints.
        /// </summary>
        public float Speed;

        /// <summary>
        /// Whether the path is circular, i.e. whether we should move towards the first waypoint
        /// after reaching the last one.
        /// </summary>
        public bool IsCircular;

        /// <summary>
        /// Whether we're moving in reverse. Always true at the beginning because the moving object
        /// will always move towards the first waypoint.
        /// </summary>
        private bool inReverse = true;

        /// <summary>
        /// The index of the current destination waypoint.
        /// </summary>
        private int currentIndex;

        /// <summary>
        /// Whether the movement is paused.
        /// </summary>
        private bool isWaiting;

        /// <summary>
        /// The last speed that we were moving with.
        /// </summary>
        private float lastSpeed;

        /// <summary>
        /// Finds the waypoints.
        /// </summary>
        private void Start()
        {
            waypoints = GetWaypoints();

            logger = new MethodLogger(nameof(WaypointFollower));
            logger.Log($"Found {waypoints.Length} waypoints");
        }

        /// <summary>
        /// Move towards the current waypoint.
        /// </summary>
        private void Update()
        {
            if (CurrentWaypoint != null && !isWaiting)
            {
                MoveTowardsWaypoint();
            }
        }

        /// <summary>
        /// Pause movement.
        /// </summary>
        private void Pause()
        {
            isWaiting = !isWaiting;
        }

        /// <summary>
        /// Move towards the current destination waypoint.
        /// </summary>
        private void MoveTowardsWaypoint()
        {
            var currentPosition = transform.position;
            var targetPosition = CurrentWaypoint.transform.position;

            // if the moving object isn't that close to the waypoint
            if (Vector3.Distance(currentPosition, targetPosition) > .1f)
            {
                // get the direction and normalise
                var directionOfTravel = targetPosition - currentPosition;
                directionOfTravel.Normalize();

                // scale the movement on each axis by the directionOfTravel vector components
                transform.Translate(
                    directionOfTravel.x * Speed * Time.deltaTime,
                    directionOfTravel.y * Speed * Time.deltaTime,
                    directionOfTravel.z * Speed * Time.deltaTime,
                    Space.World
                );
            }
            else
            {
                // If the waypoint has a pause amount then wait a bit
                if (CurrentWaypoint.WaitSeconds > 0)
                {
                    Pause();
                    Invoke(nameof(Pause), CurrentWaypoint.WaitSeconds);
                }

                // If the current waypoint has a speed change then change to it
                if (CurrentWaypoint.SpeedOut > 0)
                {
                    lastSpeed = Speed;
                    Speed = CurrentWaypoint.SpeedOut;
                }
                else if (lastSpeed != 0)
                {
                    Speed = lastSpeed;
                    lastSpeed = 0;
                }

                SetNextWaypoint();
            }
        }

        /// <summary>
        /// Compute what the next waypoint is going to be.
        /// </summary>
        private void SetNextWaypoint()
        {
            if (IsCircular)
            {
                if (!inReverse)
                {
                    currentIndex = (currentIndex + 1 >= waypoints.Length) ? 0 : currentIndex + 1;
                }
                else
                {
                    currentIndex = currentIndex == 0 ? waypoints.Length - 1 : currentIndex - 1;
                }
            }
            else
            {
                // if at the start or the end then reverse
                if ((!inReverse && currentIndex + 1 >= waypoints.Length) || (inReverse && currentIndex == 0))
                {
                    inReverse = !inReverse;
                }

                currentIndex = (!inReverse) ? currentIndex + 1 : currentIndex - 1;
            }
        }

        /// <summary>
        /// Returns the waypoints in the scene.
        /// </summary>
        private static Waypoint[] GetWaypoints()
        {
            return GameObject.FindGameObjectWithTag(Tags.WaypointsTag)
                             .GetComponentsInChildren<Waypoint>()
                             .OrderBy(w => w.transform.GetSiblingIndex())
                             .ToArray();
        }
    }
}
