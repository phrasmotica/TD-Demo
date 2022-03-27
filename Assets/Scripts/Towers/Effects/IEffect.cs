﻿using TDDemo.Assets.Scripts.Enemies;

namespace TDDemo.Assets.Scripts.Towers
{
    public interface IEffect
    {
        EffectCategory Category { get; }

        bool IsFinished { get; }

        void Start(Enemy enemy);

        void Update(Enemy enemy, float time);

        void End(Enemy enemy);
    }

    public enum EffectCategory
    {
        Slow,
    }
}
