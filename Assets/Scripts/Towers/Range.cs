using System.Linq;
using TDDemo.Assets.Scripts.Towers.Actions;
using TDDemo.Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.Events;

namespace TDDemo.Assets.Scripts.Towers
{
    public class Range : BaseBehaviour
    {
        public TowerBehaviour TowerBehaviour;

        public int RangeToDraw { get; private set; }

        private bool _towerCanBePlaced;

        private SpriteRenderer _spriteRenderer;

        public event UnityAction OnRedraw;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            OnRedraw += () =>
            {
                if (_spriteRenderer != null)
                {
                    DrawRange();
                }
            };

            TowerBehaviour.OnSelected += SetShowRange;

            TowerBehaviour.OnCanBePlaced += SetTowerCanBePlaced;

            TowerBehaviour.OnAccumulateActions += actions =>
            {
                var actionsWithRange = actions.OfType<IHasRange>();
                var range = actionsWithRange.Any() ? actionsWithRange.Max(a => a.Range) : 0;
                SetRange(range);
            };

            TowerBehaviour.OnMouseEnterEvent += () =>
            {
                if (!TowerBehaviour.IsPositioning)
                {
                    SetShowRange(true);
                }
            };

            TowerBehaviour.OnMouseExitEvent += () =>
            {
                if (!TowerBehaviour.IsPositioning && !TowerBehaviour.IsSelected)
                {
                    SetShowRange(false);
                }
            };

            logger = new MethodLogger(nameof(Range));

            SetTowerCanBePlaced(true);
        }

        public void SetTowerCanBePlaced(bool towerCanBePlaced)
        {
            _towerCanBePlaced = towerCanBePlaced;
            OnRedraw();
        }

        public void SetRange(int range)
        {
            RangeToDraw = range;
            OnRedraw();
        }

        public void DrawRange()
        {
            logger.Log($"Drawing range of {RangeToDraw}");

            _spriteRenderer.color = _towerCanBePlaced ? CanBePlacedColour : CannotBePlacedColour;

            // radius of range sprite in world space units
            var spriteSize = _spriteRenderer.sprite.bounds.size.x / 2;

            // scale required to bring sprite to size of the range
            var scale = RangeToDraw / spriteSize;

            transform.localScale = new Vector3(scale, scale, 1);
        }

        private void SetShowRange(bool isSelected)
        {
            _spriteRenderer.enabled = isSelected;
        }

        private static Color CanBePlacedColour => Color.white;

        private static Color CannotBePlacedColour => Color.red;
    }
}
