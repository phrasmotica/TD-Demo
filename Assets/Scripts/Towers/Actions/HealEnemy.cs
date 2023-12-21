using TDDemo.Assets.Scripts.Towers.Strikes;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers.Actions
{
    public class HealEnemy : StrikeProvider
    {
        [Range(1f, 3f)]
        public float Amount;

        public override IStrike CreateStrike() => new Heal(Amount)
        {
            SourceTower = SourceTower,
        };

        public override float? GetRadius() => null;
    }
}
