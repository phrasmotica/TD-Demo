using TDDemo.Assets.Scripts.Towers;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Enemies
{
    public class HealthBar : MonoBehaviour
    {
        [Range(0.5f, 3f)]
        public float HealthPeekTime;

        public SpriteRenderer EnemySprite;

        private LineRenderer _line;

        private bool _mouseIsOverEnemy;

        private TimeCounter _healthPeekCounter;

        private void Start()
        {
            _line = GetComponent<LineRenderer>();
            _line.positionCount = 2;
            _line.useWorldSpace = false;

            _healthPeekCounter = new(HealthPeekTime);
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

        public void HandleMouseEnterEnemy()
        {
            _mouseIsOverEnemy = true;
        }

        public void HandleMouseExitEnemy()
        {
            _mouseIsOverEnemy = false;
        }

        public void DrawAndPeek(Enemy enemy)
        {
            if (enemy.HealthFraction <= 0)
            {
                return;
            }

            var spriteExtentX = EnemySprite.sprite.bounds.extents.x;

            var start = -spriteExtentX;
            var width = 2 * spriteExtentX * enemy.HealthFraction;

            DrawHealthBar(start, width);
            PeekHealth();
        }

        private void DrawHealthBar(float start, float width)
        {
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
