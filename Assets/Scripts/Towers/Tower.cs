﻿using System.Collections.Generic;
using System.Linq;
using TDDemo.Assets.Scripts.Towers.Experience;
using TDDemo.Assets.Scripts.Towers.Gold;

namespace TDDemo.Assets.Scripts.Towers
{
    public class Tower
    {
        private readonly List<TowerLevel> _levels;

        private TowerState _state;
        private TimeCounter _warmupCounter;
        private int _upgradeLevel;
        private TimeCounter _upgradeCounter;

        private readonly IGoldRewardCalculator<Tower> _goldRewardCalculator;

        private readonly IXpCalculator _xpCalculator;

        public Experience.Experience Experience { get; }

        public int Level => Experience.Level;

        public Tower(List<TowerLevel> levels, GoldCalculator goldCalculator, XpCalculator xpCalculator)
        {
            _levels = levels;

            _goldRewardCalculator = CreateGoldCalculator(goldCalculator);
            _xpCalculator = CreateXpCalculator(xpCalculator);

            _state = TowerState.Positioning;
            _upgradeLevel = 0;
            Experience = new(new PokemonExperienceCurve());
        }

        public float StartWarmingUp()
        {
            _state = TowerState.Warmup;

            var baseLevel = GetBaseLevel();
            _warmupCounter = new(baseLevel.Time);
            _warmupCounter.Start();

            return baseLevel.Time;
        }

        public float Warmup(float time)
        {
            _warmupCounter.Increment(time);
            return _warmupCounter.Progress;
        }

        public void FinishWarmingUp()
        {
            _state = TowerState.Firing;

            _warmupCounter.Stop();
        }

        public float StartUpgrading()
        {
            _state = TowerState.Upgrading;

            var upgradeTime = GetUpgradeTime();
            _upgradeCounter = new(upgradeTime);
            _upgradeCounter.Start();

            return upgradeTime;
        }

        public float Upgrade(float time)
        {
            _upgradeCounter.Increment(time);
            return _upgradeCounter.Progress;
        }

        public TowerLevel FinishUpgrading()
        {
            _state = TowerState.Firing;

            _upgradeCounter.Stop();
            _upgradeLevel++;

            // make sure only the current level object is active
            for (var i = 0; i < _levels.Count; i++)
            {
                var level = _levels[i];
                level.gameObject.SetActive(i == _upgradeLevel);
            }

            return GetLevel();
        }

        public bool IsPositioning() => _state == TowerState.Positioning;

        public bool IsWarmingUp() => _state == TowerState.Warmup;

        public bool IsUpgrading() => _state == TowerState.Upgrading;

        public bool IsFiring() => _state == TowerState.Firing;

        public bool CanBeUpgraded() => IsFiring() && _upgradeLevel < _levels.Count - 1;

        private TowerLevel GetBaseLevel() => _levels.First();

        public int GetPrice() => GetBaseLevel().Price;

        private TowerLevel GetLevel() => _levels[_upgradeLevel];

        private float GetUpgradeTime()
        {
            var nextLevel = _levels[_upgradeLevel + 1];
            return nextLevel.Time;
        }

        public int? GetUpgradeCost()
        {
            if (!CanBeUpgraded())
            {
                return null;
            }

            var nextLevel = _levels[_upgradeLevel + 1];
            return nextLevel.Price;
        }

        public int GetTotalValue()
        {
            return _levels.Take(_upgradeLevel + 1).Sum(l => l.Price);
        }

        public int AddXp(int amount)
        {
            var xp = _xpCalculator.Compute(this, amount);
            Experience.Add(xp);
            return xp;
        }

        public bool TryLevelUp() => Experience.TryLevelUp();

        public int ComputeGoldReward(int baseGoldReward) => _goldRewardCalculator.Compute(this, baseGoldReward);

        private IGoldRewardCalculator<Tower> CreateGoldCalculator(GoldCalculator goldCalculator) => goldCalculator switch
        {
            GoldCalculator.LevelLinear => new LevelLinearGoldRewardCalculator(),
            _ => new DefaultGoldRewardCalculator<Tower>(),
        };

        private IXpCalculator CreateXpCalculator(XpCalculator goldCalculator) => goldCalculator switch
        {
            XpCalculator.LevelLinear => new LevelLinearXpCalculator(),
            _ => new DefaultXpCalculator(),
        };
    }

    public enum TowerState
    {
        Positioning,
        Warmup,
        Firing,
        Upgrading,
    }
}
