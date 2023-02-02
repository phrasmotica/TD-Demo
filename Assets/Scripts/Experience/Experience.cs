namespace TDDemo.Assets.Scripts.Experience
{
    public class ExperienceContainer
    {
        private readonly IExperienceCurve _experienceCurve;

        public int Level { get; private set; }

        public int CurrentXp { get; private set; }

        public int NextLevelXp => _experienceCurve.GetRequiredXpForLevel(Level + 1);

        public ExperienceContainer(IExperienceCurve experienceCurve)
        {
            _experienceCurve = experienceCurve;

            Level = 1;
        }

        public int Add(int xp)
        {
            CurrentXp += xp;
            return CurrentXp;
        }

        public bool TryLevelUp()
        {
            if (CurrentXp >= _experienceCurve.GetRequiredXpForLevel(Level + 1))
            {
                Level++;
                return true;
            }

            return false;
        }

        public float GetProgressToNextLevel()
        {
            var currentLevelXp = _experienceCurve.GetRequiredXpForLevel(Level);
            var xpGap = NextLevelXp - currentLevelXp;
            return (float) (CurrentXp - currentLevelXp) / xpGap;
        }
    }
}
