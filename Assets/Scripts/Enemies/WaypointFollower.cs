using System.Linq;
using TDDemo.Assets.Scripts.Path;
using TDDemo.Assets.Scripts.Util;
using UnityEngine;

// adapted from https://gist.github.com/Abban/42721b25cebba33c389a

namespace TDDemo.Assets.Scripts.Enemies
{
    /// <summary>
    /// Script for making an object move in straight lines along a path of waypoints.
    /// </summary>
    public class WaypointFollower : BaseBehaviour
    {
        /// <summary>
        /// The waypoints to follow.
        /// </summary>
        private Waypoint[] _waypoints;

        /// <summary>
        /// The current destination waypoint.
        /// </summary>
        private Waypoint CurrentWaypoint => _waypoints?.Any() ?? false ? _waypoints[_currentIndex] : null;

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
        private bool _inReverse = true;

        /// <summary>
        /// The index of the current destination waypoint.
        /// </summary>
        private int _currentIndex;

        /// <summary>
        /// Whether the movement is paused.
        /// </summary>
        private bool _isWaiting;

        /// <summary>
        /// The last speed that we were moving with.
        /// </summary>
        private float _lastSpeed;

        /// <summary>
        /// Finds the waypoints.
        /// </summary>
        private void Start()
        {
            _waypoints = GetWaypoints();

            logger = new MethodLogger(nameof(WaypointFollower));
            logger.Log($"Found {_waypoints.Length} waypoints");
        }

        /// <summary>
        /// Move towards the current waypoint.
        /// </summary>
        private void Update()
        {
            if (CurrentWaypoint != null && !_isWaiting)
            {
                MoveTowardsWaypoint();
            }
        }

        /// <summary>
        /// Pause movement.
        /// </summary>
        private void Pause()
        {
            _isWaiting = !_isWaiting;
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
                    _lastSpeed = Speed;
                    Speed = CurrentWaypoint.SpeedOut;
                }
                else if (_lastSpeed != 0)
                {
                    Speed = _lastSpeed;
                    _lastSpeed = 0;
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
                if (!_inReverse)
                {
                    _currentIndex = _currentIndex + 1 >= _waypoints.Length ? 0 : _currentIndex + 1;
                }
                else
                {
                    _currentIndex = _currentIndex == 0 ? _waypoints.Length - 1 : _currentIndex - 1;
                }
            }
            else
            {
                // if at the start or the end then reverse
                if (!_inReverse && _currentIndex + 1 >= _waypoints.Length || _inReverse && _currentIndex == 0)
                {
                    _inReverse = !_inReverse;
                }

                _currentIndex = !_inReverse ? _currentIndex + 1 : _currentIndex - 1;
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
