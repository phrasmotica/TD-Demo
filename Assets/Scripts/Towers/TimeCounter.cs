namespace TDDemo.Assets.Scripts.Towers
{
    /// <summary>
    /// Class for storing information about time that has elapsed towards a condition being met.
    /// </summary>
    public class TimeCounter
    {
        private readonly float _totalTimeSeconds;

        private float _elapsedSeconds;

        public float Progress => _elapsedSeconds / _totalTimeSeconds;

        public bool IsStarted { get; private set; }

        public bool IsFinished => _elapsedSeconds >= _totalTimeSeconds;

        public TimeCounter(float totalTimeSeconds)
        {
            _totalTimeSeconds = totalTimeSeconds;
        }

        public void Increment(float timeSeconds)
        {
            if (!IsStarted)
            {
                IsStarted = true;
            }

            _elapsedSeconds += timeSeconds;
        }

        public void Reset() => _elapsedSeconds = 0;
    }
}
