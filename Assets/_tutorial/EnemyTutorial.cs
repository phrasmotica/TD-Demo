using UnityEngine;

namespace TDDemo
{
    public class EnemyTutorial : MonoBehaviour
    {
        public float Speed = 3f;

        private Transform _target;

        private int _waypointIndex = 0;

        private void Start()
        {
            _target = Waypoints.Points[0];
        }

        private void Update()
        {
            var dir = _target.position - transform.position;
            transform.Translate(Speed * Time.deltaTime * dir.normalized, Space.World);

            if (Vector3.Distance(transform.position, _target.position) <= 0.01f * Speed)
            {
                GetNextWaypoint();
            }
        }

        private void GetNextWaypoint()
        {
            if (_waypointIndex >= Waypoints.Points.Length - 1)
            {
                Destroy(gameObject);
                return;
            }

            _target = Waypoints.Points[++_waypointIndex];
        }
    }
}
