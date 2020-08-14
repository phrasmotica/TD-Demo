using System;
using System.Collections;
using Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Towers
{
    public class Tower : MonoBehaviour
    {
        /// <summary>
        /// The tower's state.
        /// </summary>
        public TowerState State;

        /// <summary>
        /// The tower's price.
        /// </summary>
        [Range(0, 5)]
        public int Price;

        /// <summary>
        /// The time taken in seconds for this tower to warm up.
        /// </summary>
        [Range(0, 5)]
        public int WarmupTime;

        /// <summary>
        /// Delegate to run on placing the tower.
        /// </summary>
        public Action<int> OnPlace { private get; set; }

        /// <summary>
        /// The tower's initial Z position.
        /// </summary>
        private float InitialZPos;

        // Start is called before the first frame update
        private void Start()
        {
            InitialZPos = transform.position.z;
            State = TowerState.Positioning;
        }

        // Update is called once per frame
        private void Update()
        {
            using (var logger = new MethodLogger(nameof(Tower)))
            {
                if (State == TowerState.Positioning)
                {
                    var mousePosition = Input.mousePosition;
                    var worldPoint = Camera.main.ScreenToWorldPoint(mousePosition);
                    transform.position = new Vector3(worldPoint.x, worldPoint.y, InitialZPos);

                    if (Input.GetMouseButtonUp((int) MouseButton.LeftMouse))
                    {
                        logger.Log($"Placed tower at {worldPoint}");
                        OnPlace(Price);
                        DoWarmup();
                    }
                }
            }
        }

        /// <summary>
        /// Make the tower warm up before being ready to fire.
        /// </summary>
        private void DoWarmup()
        {
            State = TowerState.Warmup;
            StartCoroutine(Warmup());
        }


        private IEnumerator Warmup()
        {
            Debug.Log($"Tower warming up for {WarmupTime} seconds");
            yield return new WaitForSeconds(WarmupTime);

            Debug.Log($"Tower ready");
            State = TowerState.Firing;
        }
    }

    /// <summary>
    /// Possible states for a tower to occupy.
    /// </summary>
    public enum TowerState
    {
        Positioning,
        Warmup,
        Firing,
        Upgrading,
    }
}
