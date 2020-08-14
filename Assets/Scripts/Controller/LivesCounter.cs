using UnityEngine;
using UnityEngine.UI;

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
        /// The current number of lives.
        /// </summary>
        private int Lives
        {
            get
            {
                return lives;
            }
            set
            {
                lives = value;
                LivesText.text = $"Lives: {lives}";
            }
        }
        private int lives;

        /// <summary>
        /// The text used to display the lives.
        /// </summary>
        public Text LivesText;

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
    }
}
