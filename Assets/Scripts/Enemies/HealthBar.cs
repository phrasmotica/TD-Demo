﻿using TDDemo.Assets.Scripts.Util;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Enemies
{
    /// <summary>
    /// Draws the health of an enemy.
    /// </summary>
    public class HealthBar : BaseBehaviour
    {
        /// <summary>
        /// The offset with which to render the health bar.
        /// </summary>
        [Range(0.1f, 0.5f)]
        public float OffsetY;

        /// <summary>
        /// The time in seconds to draw the enemy's health for when it's hit.
        /// </summary>
        [Range(0.5f, 3f)]
        public float PeekTime;

        /// <summary>
        /// The sprite renderer.
        /// </summary>
        private SpriteRenderer _sprite;

        /// <summary>
        /// The line renderer.
        /// </summary>
        private LineRenderer _line;

        /// <summary>
        /// Whether the mouse cursor is over the enemy.
        /// </summary>
        private bool _mouseIsOverEnemy;

        /// <summary>
        /// The time in seconds left to draw the enemy's health for.
        /// </summary>
        private float _remainingPeekTime;

        /// <summary>
        /// Gets whether to draw the health bar.
        /// </summary>
        private bool ShouldDrawHealthBar => _remainingPeekTime > 0 || _mouseIsOverEnemy;

        /// <summary>
        /// Gets the health fraction of the enemy.
        /// </summary>
        private float HealthFraction => GetComponentInParent<Enemy>().HealthFraction;

        /// <summary>
        /// Initialise the script.
        /// </summary>
        private void Start()
        {
            _line = GetComponent<LineRenderer>();
            _line.positionCount = 2;
            _line.useWorldSpace = false;

            _sprite = GetComponentInParent<SpriteRenderer>();

            logger = new MethodLogger(nameof(HealthBar));

            DrawHealthBar(HealthFraction);
        }

        /// <summary>
        /// Draw the health if necessary.
        /// </summary>
        private void Update()
        {
            if (_remainingPeekTime > 0)
            {
                _remainingPeekTime -= Time.deltaTime;
            }

            // draw health bar if we're in the peek period or if mouse is over the enemy
            _line.forceRenderingOff = !ShouldDrawHealthBar;
        }

        /// <summary>
        /// Mouse is over the collider so draw the health.
        /// </summary>
        private void OnMouseEnter()
        {
            logger.Log("Mouse is over the enemy");
            _mouseIsOverEnemy = true;
        }

        /// <summary>
        /// Mouse is no longer over the collider so hide the health.
        /// </summary>
        private void OnMouseExit()
        {
            logger.Log("Mouse is no longer over the enemy");
            _mouseIsOverEnemy = false;
        }

        /// <summary>
        /// Draws the health bar.
        /// </summary>
        private void DrawHealthBar(float healthFraction)
        {
            var spriteExtentX = _sprite.sprite.bounds.extents.x;
            var spriteExtentY = _sprite.sprite.bounds.extents.y;

            var start = -spriteExtentX;
            var width = 2 * spriteExtentX * healthFraction;

            _line.SetPosition(0, new Vector3(start, spriteExtentY + OffsetY, 0));
            _line.SetPosition(1, new Vector3(start + width, spriteExtentY + OffsetY, 0));
        }

        /// <summary>
        /// Show the enemy's health briefly.
        /// </summary>
        public void PeekHealth()
        {
            _remainingPeekTime = PeekTime;
        }

        /// <summary>
        /// Show the enemy's health briefly.
        /// </summary>
        public void PeekHealth(float newHealthFraction)
        {
            DrawHealthBar(newHealthFraction);
            PeekHealth();
        }
    }
}
