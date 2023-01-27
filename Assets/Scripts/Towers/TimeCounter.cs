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

        public bool IsRunning { get; private set; }

        public bool IsFinished => _elapsedSeconds >= _totalTimeSeconds;

        public TimeCounter(float totalTimeSeconds)
        {
            _totalTimeSeconds = totalTimeSeconds;
        }

        public void Increment(float timeSeconds)
        {
            if (IsRunning)
            {
                _elapsedSeconds += timeSeconds;
            }
        }

        public void Start() => IsRunning = true;

        public void Reset()
        {
            IsRunning = false;
            _elapsedSeconds = 0;
        }
    }
}
