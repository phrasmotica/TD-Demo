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

            Enemy.OnHurt += DrawAndPeek;
            Enemy.OnHeal += DrawAndPeek;
        }

        private void Update()
        {
            if (!_healthPeekCounter.IsFinished)
            {
                _healthPeekCounter.Increment(Time.deltaTime);
            }

            // show health bar if we're in the peek period or if mouse is over the enemy
            _line.forceRenderingOff = !ShouldDrawHealthBar();
        }

        private void OnMouseEnter()
        {
            _mouseIsOverEnemy = true;
        }

        private void OnMouseExit()
        {
            _mouseIsOverEnemy = false;
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

        private void PeekHealth() => _healthPeekCounter.Reset();

        private bool ShouldDrawHealthBar() => !_healthPeekCounter.IsFinished || _mouseIsOverEnemy;
    }
}
