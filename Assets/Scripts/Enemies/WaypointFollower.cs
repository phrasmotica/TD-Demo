using System.Linq;
using TDDemo.Assets.Scripts.Path;
using UnityEngine;

// adapted from https://gist.github.com/Abban/42721b25cebba33c389a

namespace TDDemo.Assets.Scripts.Enemies
{
    /// <summary>
    /// Script for making an object move in straight lines along a path of waypoints.
    /// </summary>
    public class WaypointFollower : MonoBehaviour
    {
        public Waypoint[] Waypoints { get; set; }

        public float Speed;

        public bool IsParalysed;

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

        private void Update()
        {
            if (!IsParalysed && !_isWaiting && GetCurrentWaypoint() != null)
            {
                MoveTowardsWaypoint();
            }
        }

        private void Pause()
        {
            _isWaiting = !_isWaiting;
        }

        private void MoveTowardsWaypoint()
        {
            var currentPosition = transform.position;

            var targetWaypoint = GetCurrentWaypoint();
            var targetPosition = targetWaypoint.transform.position;

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
                if (targetWaypoint.WaitSeconds > 0)
                {
                    Pause();
                    Invoke(nameof(Pause), targetWaypoint.WaitSeconds);
                }

                // If the current waypoint has a speed change then change to it
                if (targetWaypoint.SpeedOut > 0)
                {
                    _lastSpeed = Speed;
                    Speed = targetWaypoint.SpeedOut;
                }
                else if (_lastSpeed != 0)
                {
                    Speed = _lastSpeed;
                    _lastSpeed = 0;
                }

                SetNextWaypoint();
            }
        }

        private void SetNextWaypoint()
        {
            if (IsCircular)
            {
                if (!_inReverse)
                {
                    _currentIndex = _currentIndex + 1 >= Waypoints.Length ? 0 : _currentIndex + 1;
                }
                else
                {
                    _currentIndex = _currentIndex == 0 ? Waypoints.Length - 1 : _currentIndex - 1;
                }
            }
            else
            {
                // if at the start or the end then reverse
                if (!_inReverse && _currentIndex + 1 >= Waypoints.Length || _inReverse && _currentIndex == 0)
                {
                    _inReverse = !_inReverse;
                }

                _currentIndex = !_inReverse ? _currentIndex + 1 : _currentIndex - 1;
            }
        }

        private Waypoint GetCurrentWaypoint() => Waypoints.Any() ? Waypoints[_currentIndex] : null;
    }
}
