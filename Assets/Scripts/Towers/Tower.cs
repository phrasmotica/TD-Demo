using System.Collections.Generic;
using System.Linq;

namespace TDDemo.Assets.Scripts.Towers
{
    public class Tower
    {
        private readonly int _price;
        private readonly TimeCounter _warmupCounter;
        private readonly List<TowerLevel> _levels;

        private TowerState _state;
        private int _upgradeLevel;
        private TimeCounter _upgradeCounter;

        public float WarmupProgress => _warmupCounter.Progress;

        public float UpgradeProgress => _upgradeCounter.Progress;

        /// <summary>
        /// Initialise the script.
        /// </summary>
        public Tower(int price, float warmupTime, List<TowerLevel> levels)
        {
            _price = price;
            _warmupCounter = new TimeCounter(warmupTime);
            _levels = levels;

            _state = TowerState.Positioning;
            _upgradeLevel = 0;
        }

        public void StartWarmingUp()
        {
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

        public float StartUpgrading()
        {
            _state = TowerState.Upgrading;

            var upgradeTime = GetUpgradeTime();
            _upgradeCounter = new TimeCounter(upgradeTime);
            return upgradeTime;
        }

        public void Upgrade(float time)
        {
            _upgradeCounter.Increment(time);
        }

        public TowerLevel FinishUpgrading()
        {
            _upgradeCounter.Reset();
            _state = TowerState.Firing;

            return _levels[++_upgradeLevel];
        }

        public bool IsPositioning() => _state == TowerState.Positioning;

        public bool IsWarmingUp() => _state == TowerState.Warmup;

        public bool IsUpgrading() => _state == TowerState.Upgrading;

        public bool IsFiring() => _state == TowerState.Firing;

        public bool CanBeUpgraded() => IsFiring() && _upgradeLevel < _levels.Count - 1;

        public TowerLevel GetLevel()
        {
            return _levels[_upgradeLevel];
        }

        public int GetUpgradeCost()
        {
            if (_upgradeLevel >= _levels.Count - 1)
            {
                return 0;
            }

            var nextLevel = _levels[_upgradeLevel + 1];
            return nextLevel.UpgradeCost;
        }

        public int GetTotalValue()
        {
            var upgradeCosts = _levels.Skip(1).Take(_upgradeLevel).Sum(l => l.UpgradeCost);
            return _price + upgradeCosts;
        }

        private float GetUpgradeTime()
        {
            var nextLevel = _levels[_upgradeLevel + 1];
            return nextLevel.UpgradeTime;
        }
    }
}
