using TDDemo.Assets.Scripts.Towers;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Enemies
{
    public class HealthBar : MonoBehaviour
    {
        public Enemy Enemy;

        [Range(0.5f, 3f)]
        public float HealthPeekTime;

        private LineRenderer _line;

        private bool _mouseIsOverEnemy;

        private TimeCounter _healthPeekCounter;

        private void Start()
        {
            _line = GetComponent<LineRenderer>();
            _line.positionCount = 2;
            _line.useWorldSpace = false;

            _healthPeekCounter = new(HealthPeekTime);

            Enemy.OnMouseEnterEvent += () => _mouseIsOverEnemy = true;
            Enemy.OnMouseExitEvent += () => _mouseIsOverEnemy = false;

            Enemy.OnHurt += DrawAndPeek;
            Enemy.OnHeal += DrawAndPeek;
        }

        private void Update()
        {
            if (_healthPeekCounter.IsFinished)
            {
                _healthPeekCounter.Stop();
            }
            else
            {
                _healthPeekCounter.Increment(Time.deltaTime);
            }

            _line.forceRenderingOff = !ShouldDrawHealthBar();
        }

        private void DrawAndPeek(float healthFraction)
        {
            DrawHealthBar(healthFraction);
            PeekHealth();
        }

        private void DrawHealthBar(float healthFraction)
        {
            var enemySprite = Enemy.GetComponent<SpriteRenderer>();
            var spriteExtentX = enemySprite.sprite.bounds.extents.x;

            var start = -spriteExtentX;
            var width = 2 * spriteExtentX * healthFraction;

            _line.SetPosition(0, new Vector3(start, 0, 0));
            _line.SetPosition(1, new Vector3(start + width, 0, 0));
        }

        private void PeekHealth() => _healthPeekCounter.Start();

        private bool ShouldDrawHealthBar()
        {
            var inPeekPeriod = _healthPeekCounter.IsRunning && !_healthPeekCounter.IsFinished;
            return inPeekPeriod || _mouseIsOverEnemy;
        }
    }
}
