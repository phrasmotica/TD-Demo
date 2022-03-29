using TDDemo.Assets.Scripts.Towers.Effects;
using TDDemo.Assets.Scripts.Util;
using UnityEngine;

namespace TDDemo.Assets.Scripts.Towers
{
    public class EffectProgressBar : MonoBehaviour
    {
        private LineRenderer _line;

        public IEffect Effect { get; set; }

        public float StartPosX { get; set; }

        public float MaxWidth { get; set; }

        public bool IsFinished => Effect.IsFinished;

        private void Start()
        {
            _line = GetComponent<LineRenderer>();
            _line.positionCount = 2;
            _line.useWorldSpace = false;

            var colour = GetColour();
            _line.startColor = colour;
            _line.endColor = colour;
        }

        private void Update()
        {
            var shouldDraw = ShouldDrawBar();
            _line.forceRenderingOff = !shouldDraw;

            if (shouldDraw)
            {
                DrawProgressBar();
            }
        }

        private void DrawProgressBar()
        {
            var width = MaxWidth * (1 - Effect.Progress);

            _line.SetPosition(0, new Vector3(StartPosX, 0, 0));
            _line.SetPosition(1, new Vector3(StartPosX + width, 0, 0));
        }

        private bool ShouldDrawBar() => !Effect.IsFinished;

        private Color GetColour() => Effect.Category switch
        {
            EffectCategory.Slow => ColourHelper.SlowEffect,
            EffectCategory.Paralyse => ColourHelper.ParalyseEffect,
            _ => Color.cyan,
        };
    }
}
