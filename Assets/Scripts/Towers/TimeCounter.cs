namespace TDDemo.Assets.Scripts.Towers
{
    /// <summary>
    /// Class for storing information about time that has elapsed towards a condition being met.
    /// </summary>
    public class TimeCounter
    {
        /// <summary>
        /// The time taken in seconds for this counter to finish.
        /// </summary>
        private readonly float _totalTime;

        /// <summary>
        /// The time that has elapsed in seconds.
        /// </summary>
        private float _elapsed;

        public float Progress => _elapsed / _totalTime;

        public bool IsFinished => Progress >= 1;

        public TimeCounter(float totalTime)
        {
            _totalTime = totalTime;
        }

        public void Increment(float inc)
        {
            _elapsed += inc;
        }

        public void Reset()
        {
            _elapsed = 0;
        }
    }
}
