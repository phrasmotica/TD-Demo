using TDDemo.Assets.Scripts.Towers.Strikes;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers.Actions
{
    public class DamageEnemy : StrikeProvider
    {
        [Range(1f, 10f)]
        public float Amount;

        public override IStrike CreateStrike() => new Damage(Amount)
        {
            SourceTower = SourceTower,
        };

        public override float? GetRadius() => null;
    }
}
