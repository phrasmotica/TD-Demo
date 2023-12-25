using TDDemo.Assets.Scripts.Enemies;
using TDDemo.Assets.Scripts.Towers;
using TDDemo.Assets.Scripts.Towers.Effects;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Distractions
{
    public class Distract : IEffect
    {
        private readonly float _duration;

        private TimeCounter _counter;

        private Vector3 _originalForward;

        public TowerBehaviour SourceTower { get; set; }

        public DistractionSource Source {  get; set; }

        public EffectCategory Category => EffectCategory.Distract;

        public float Progress => _counter.Progress;

        public bool IsFinished => _counter.IsFinished;

        public Distract(float duration)
        {
            _duration = duration;
        }

        public void Start(Enemy enemy)
        {
            var movement = enemy.GetComponent<WaypointFollower>();
            movement.IsParalysed = true;

            if (enemy.TryGetComponent<Animator>(out var animator))
            {
                animator.enabled = false;
            }

            // "right" is the direction the enemy looks "forward" in 2D
            _originalForward = enemy.transform.right;

            // enemy looks at source of distraction
            enemy.SpriteTransform.right = Source.transform.position - enemy.SpriteTransform.position;

            _counter = new(_duration);
            _counter.Start();
        }

        public void Update(Enemy enemy, float time)
        {
            _counter.Increment(time);
        }

        public void End(Enemy enemy)
        {
            var movement = enemy.GetComponent<WaypointFollower>();
            movement.IsParalysed = false;

            if (enemy.TryGetComponent<Animator>(out var animator))
            {
                animator.enabled = true;
            }

            // enemy continues movement as normal
            enemy.SpriteTransform.right = _originalForward;
        }
    }
}
