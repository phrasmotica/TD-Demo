using UnityEngine;

namespace TDDemo.Assets.Scripts.Enemies
{
    /// <summary>
    /// Draws the health of an enemy.
    /// </summary>
    public class HealthBar : MonoBehaviour
    {
        public Enemy Enemy;

        /// <summary>
        /// The time in seconds to draw the enemy's health for when it's hit.
        /// </summary>
        [Range(0.5f, 3f)]
        public float PeekTime;

        private LineRenderer _line;

        private bool _mouseIsOverEnemy;

        /// <summary>
        /// The time in seconds left to draw the enemy's health for.
        /// </summary>
        private float _remainingPeekTime;

        private void Start()
        {
            _line = GetComponent<LineRenderer>();
            _line.positionCount = 2;
            _line.useWorldSpace = false;

            Enemy.OnHurt += newHealthFraction =>
            {
                DrawHealthBar(newHealthFraction);
                PeekHealth();
            };
        }

        private void Update()
        {
            if (_remainingPeekTime > 0)
            {
                _remainingPeekTime -= Time.deltaTime;
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

        private void DrawHealthBar(float healthFraction)
        {
            var enemySprite = Enemy.GetComponent<SpriteRenderer>();
            var spriteExtentX = enemySprite.sprite.bounds.extents.x;

            var start = -spriteExtentX;
            var width = 2 * spriteExtentX * healthFraction;

            var posY = transform.position.y;

            _line.SetPosition(0, new Vector3(start, posY, 0));
            _line.SetPosition(1, new Vector3(start + width, posY, 0));
        }

        private void PeekHealth()
        {
            _remainingPeekTime = PeekTime;
        }

        private bool ShouldDrawHealthBar() => _remainingPeekTime > 0 || _mouseIsOverEnemy;
    }
}
