using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers
{
    // TODO: absorb this into ShootEnemy script
    public class ProjectileSpecs : MonoBehaviour
    {
        public GameObject ProjectilePrefab;

        [Range(1, 10)]
        public int ProjectileSpeed;

        [Range(1, 10)]
        public int FireRate;

        [Range(1, 10)]
        public int Range;
    }
}
