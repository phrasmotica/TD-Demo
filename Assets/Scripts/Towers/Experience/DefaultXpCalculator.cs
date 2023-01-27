namespace TDDemo.Assets.Scripts.Towers.Experience
{
    public class DefaultXpCalculator : IXpCalculator
    {
        public int Compute(Tower tower, int baseXp) => baseXp;
    }
}
