using System.Linq;
using TDDemo.Assets.Scripts.Towers.Actions;
using TDDemo.Assets.Scripts.Util;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers
{
    public class Range : BaseBehaviour
    {
        public int RangeToDraw { get; private set; }

        private bool _towerCanBePlaced;

        public SpriteRenderer SpriteRenderer;

        private void Awake()
        {
            logger = new MethodLogger(nameof(Range));

            SetTowerCanBePlaced(true);
        }

        public void Recompute(ITowerAction[] actions)
        {
            var actionsWithRange = actions.OfType<IHasRange>().Cast<IHasRange>();
            var range = actionsWithRange.Any() ? actionsWithRange.Max(a => a.GetRange()) : 0;

            SetRange(range);
        }

        public void HandleTowerMouseEnter(TowerBehaviour tower)
        {
            if (!tower.IsPositioning())
            {
                SetShowRange(true);
            }
        }

        public void HandleTowerMouseExit(TowerBehaviour tower)
        {
            if (!tower.IsPositioning() && !tower.IsSelected)
            {
                SetShowRange(false);
            }
        }

        public void SetTowerCanBePlaced(bool towerCanBePlaced)
        {
            _towerCanBePlaced = towerCanBePlaced;
            DrawRange();
        }

        public void SetRange(int range)
        {
            RangeToDraw = range;
            DrawRange();
        }

        public void DrawRange()
        {
            if (SpriteRenderer != null)
            {
                logger.Log($"Drawing range of {RangeToDraw}");

                SpriteRenderer.color = _towerCanBePlaced ? CanBePlacedColour : CannotBePlacedColour;

                // radius of range sprite in world space units
                var spriteSize = SpriteRenderer.sprite.bounds.size.x / 2;

                // scale required to bring sprite to size of the range
                var scale = RangeToDraw / spriteSize;

                transform.localScale = new Vector3(scale, scale, 1);
            }
        }

        public void SetShowRange(bool isSelected)
        {
            SpriteRenderer.enabled = isSelected;
        }

        private static Color CanBePlacedColour => Color.white;

        private static Color CannotBePlacedColour => Color.red;
    }
}
