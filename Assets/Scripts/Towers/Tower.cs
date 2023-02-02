using TDDemo.Assets.Scripts.Experience;
using TDDemo.Assets.Scripts.Towers.Experience;
using TDDemo.Assets.Scripts.Towers.Gold;

namespace TDDemo.Assets.Scripts.Towers
{
    public class Tower
    {
        private TowerState _state;
        private TimeCounter _warmupCounter;
        private TimeCounter _upgradeCounter;

        private readonly IGoldRewardCalculator<Tower> _goldRewardCalculator;

        private readonly IXpCalculator _xpCalculator;

        public int UpgradeLevel { get; private set; }

        public Scripts.Experience.Experience Experience { get; }

        public int Level => Experience.Level;

        public Tower(GoldCalculator goldCalculator, XpCalculator xpCalculator)
        {
            _goldRewardCalculator = CreateGoldCalculator(goldCalculator);
            _xpCalculator = CreateXpCalculator(xpCalculator);

            _state = TowerState.Positioning;
            UpgradeLevel = 0;
            Experience = new(new PokemonExperienceCurve());
        }

        public void StartWarmingUp(float warmupTime)
        {
            _state = TowerState.Warmup;

            _warmupCounter = new(warmupTime);
            _warmupCounter.Start();
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

        public void StartUpgrading(float upgradeTime)
        {
            _state = TowerState.Upgrading;

            _upgradeCounter = new(upgradeTime);
            _upgradeCounter.Start();
        }

        public float Upgrade(float time)
        {
            _upgradeCounter.Increment(time);
            return _upgradeCounter.Progress;
        }

        public int FinishUpgrading()
        {
            _state = TowerState.Firing;
            _upgradeCounter.Stop();

            return ++UpgradeLevel;
        }

        public bool IsPositioning() => _state == TowerState.Positioning;

        public bool IsWarmingUp() => _state == TowerState.Warmup;

        public bool IsUpgrading() => _state == TowerState.Upgrading;

        public bool IsFiring() => _state == TowerState.Firing;

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
