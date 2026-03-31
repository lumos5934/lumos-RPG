using System;
using System.Collections.Generic;

namespace LumosLib.RPG
{
    public static class UnitEffectSystem
    {
        private static readonly List<IUnitEffectModifier> _modifiers = new();
        private static readonly Stack<UnitEffectContext> _contextPool = new();
        
        
        public static void Apply(Action<UnitEffectContext> onSetup)
        {
            var context = _contextPool.Count > 0 ? _contextPool.Pop() : new UnitEffectContext();
            
            try 
            {
                onSetup?.Invoke(context);
                
                if (context.Target == null)
                {
                    DebugUtil.LogWarning($"{context.GetType().Name} : Missing Target", "UnitEffectSystem");
                    return;
                }

                if (context.Source == null)
                {
                    DebugUtil.LogWarning($"{context.GetType().Name} : Missing Source", "UnitEffectSystem");
                    return;
                }
                
                if (context.Effects == null ||
                    context.Effects.Count == 0)
                {
                    DebugUtil.LogWarning($"{context.GetType().Name} : Missing Effects", "UnitEffectSystem");
                    return;
                }
                
                
                foreach (var modifier in _modifiers)
                {
                    modifier.Modify(context);
                }


                var targetVitals = context.Target.Vitals;
                var targetStats = context.Target.Stats;
        
                foreach (var effect in context.Effects)
                {
                    if (effect.TargetVitalID >= 0)
                    {
                        var targetVital = targetVitals.Get(effect.TargetVitalID);
                        targetVital?.Apply(effect.FinalValue);
                    }

                    if (effect.TargetStatID >= 0)
                    {
                        var targetStat = targetStats.Get(effect.TargetStatID);
                        targetStat?.SetBaseValue(effect.FinalValue);
                    }
                }

                context.Target.OnApplyEffect(context);
            }
            finally
            {
                context.Reset();
                _contextPool.Push(context);
            }
        }

        
        public static void AddModifier(IUnitEffectModifier modifier)
        {
            _modifiers.Add(modifier);
            _modifiers.Sort((a, b) => a.Order.CompareTo(b.Order));
        }
    }
}
