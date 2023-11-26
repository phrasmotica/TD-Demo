using System.Collections.Generic;
using System.Linq;
using TDDemo.Assets.Scripts.Towers;
using TDDemo.Assets.Scripts.Towers.Effects;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Enemies
{
    public class EffectProgressBarSet : MonoBehaviour
    {
        public GameObject EffectProgressPrefab;

        private List<EffectProgressBar> _progressBars;

        private void Start()
        {
            _progressBars = new List<EffectProgressBar>();
        }

        private void Update()
        {
            CleanUpFinishedBars();
        }

        public void AddEffect(Enemy enemy, IEffect effect)
        {
            var bar = Instantiate(EffectProgressPrefab, transform).GetComponent<EffectProgressBar>();

            // move further down depending on the number of effects present
            var translateY = (float) -(_progressBars.Count * 0.2);
            bar.transform.localPosition = new Vector3(0, translateY, 0);

            bar.Effect = effect;

            var enemySprite = enemy.GetComponent<SpriteRenderer>().sprite;
            bar.StartPosX = -enemySprite.bounds.extents.x;
            bar.MaxWidth = 2 * enemySprite.bounds.extents.x;

            _progressBars.Add(bar);
        }

        private void CleanUpFinishedBars()
        {
            // clean up progress bars for finished effects
            foreach (var bar in _progressBars)
            {
                if (bar.IsFinished)
                {
                    Destroy(bar.gameObject);
                }
            }

            if (_progressBars.Any(p => p.IsFinished))
            {
                _progressBars = _progressBars.Where(e => !e.IsFinished).ToList();

                // refresh progress bar positions so they're not hovering in mid-air
                for (var i = 0; i < _progressBars.Count; i++)
                {
                    var bar = _progressBars[i];
                    var translateY = (float) -(i * 0.2);
                    bar.transform.localPosition = new Vector3(0, translateY, 0);
                }
            }
        }
    }
}
