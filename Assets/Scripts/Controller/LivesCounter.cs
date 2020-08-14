using UnityEngine;

namespace Assets.Scripts.Controller
{
    public class LivesCounter : MonoBehaviour
    {
        /// <summary>
        /// The number of starting lives.
        /// </summary>
        [Range(1, 50)]
        public int StartingLives;

        /// <summary>
        /// The number of lives.
        /// </summary>
        private int Lives;

        /// <summary>
        /// Start is called before the first frame update.
        /// </summary>
        private void Start()
        {
            Lives = StartingLives;
        }

        /// <summary>
        /// Adds the given amount of lives to the current count.
        /// </summary>
        public void AddLives(int amount)
        {
            Lives += amount;
        }

        /// <summary>
        /// Renders a GUI.
        /// </summary>
        public void OnGUI()
        {
            GUI.Label(new Rect(0, 60, 160, 30), $"Lives {Lives}");
        }
    }
}
