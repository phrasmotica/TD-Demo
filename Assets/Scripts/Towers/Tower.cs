namespace TDDemo.Assets.Scripts.Towers
{
    public class Tower
    {
        private readonly int _price;
        private readonly int _upgradePrice;
        private readonly TimeCounter _warmupCounter;
        private readonly TimeCounter _upgradeCounter;
        private readonly int _maxUpgradeLevel;

        public int Damage { get; private set; }

        private TowerState _state;
        private int _upgradeLevel;

        public float WarmupProgress => _warmupCounter.Progress;

        public float UpgradeProgress => _upgradeCounter.Progress;

        public int TotalValue { get; private set; }

        /// <summary>
        /// Initialise the script.
        /// </summary>
        public Tower(int price, int upgradePrice, float warmupTime, float upgradeTime, int maxUpgradeLevel)
        {
            _price = price;
            _upgradePrice = upgradePrice;
            _warmupCounter = new TimeCounter(warmupTime);
            _upgradeCounter = new TimeCounter(upgradeTime);
            _maxUpgradeLevel = maxUpgradeLevel;

            Damage = 1;

            _state = TowerState.Positioning;
            _upgradeLevel = 0;
        }

        public void StartWarmingUp()
        {
            TotalValue += _price;
            _state = TowerState.Warmup;
        }

        public void Warmup(float time)
        {
            _warmupCounter.Increment(time);
        }

        public void FinishWarmingUp()
        {
            _warmupCounter.Reset();
            _state = TowerState.Firing;
        }

        public void StartUpgrading()
        {
            _state = TowerState.Upgrading;
        }

        public void Upgrade(float time)
        {
            _upgradeCounter.Increment(time);
        }

        public int FinishUpgrading()
        {
            TotalValue += _upgradePrice;
            _state = TowerState.Firing;
            _upgradeCounter.Reset();
            return ++_upgradeLevel;
        }

        public bool IsPositioning() => _state == TowerState.Positioning;

        public bool IsWarmingUp() => _state == TowerState.Warmup;

        public bool IsUpgrading() => _state == TowerState.Upgrading;

        public bool IsFiring() => _state == TowerState.Firing;

        public bool CanBeUpgraded() => IsFiring() && _upgradeLevel < _maxUpgradeLevel;

        public bool IsBaseLevel() => _upgradeLevel <= 0;
    }
}
