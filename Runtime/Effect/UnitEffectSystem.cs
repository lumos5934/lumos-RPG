using System.Collections.Generic;

namespace LumosLib.RPG
{
    public static class UnitEffectSystem
    {
        private static List<IUnitEffectModifier> _modifiers = new();
        private static List<IUnitEffectFeedback> _feedbacks = new();
        private static Stack<UnitEffectContext> _contextPool = new();

        
        public static UnitEffectContext GetContext(IUnit source, IUnit target, List<UnitEffect> effects)
        {
            var ctx = _contextPool.Count > 0 ? _contextPool.Pop() : new UnitEffectContext();
            ctx.Reset();
            ctx.Source = source;
            ctx.Target = target;
            ctx.Effects = new List<UnitEffect>(effects);
            
            return ctx;
        }
        
        
        public static void Apply(UnitEffectContext ctx)
        {
            var source = ctx.Source;
            var target = ctx.Target;
            
            if (source == null || 
                target == null) 
                return;

            
            foreach (var modifier in _modifiers)
            {
                modifier.Modify(ctx);
            }
            
            
            var vitals =  target.Vitals;
            
            foreach (var effect in ctx.Effects)
            {
                var vital = vitals.Get(effect.VitalTypeID);
                if (vital != null)
                {
                    var value = effect.Value;

                    if (effect.IsNegative)
                    {
                        value *= -1f;
                    }
                    
                    vitals.Apply(effect.VitalTypeID, value);
                }
            }
            
            
            ctx.Target.OnApplyEffect(ctx);
            
            foreach (var feedback in _feedbacks)
            {
                feedback.Apply(ctx);
            }
            
            ctx.Reset();
            _contextPool.Push(ctx);
        }

        
        public static void RegisterModifier(IUnitEffectModifier modifier)
        {
            _modifiers.Add(modifier);
            _modifiers.Sort((a, b) => a.Priority.CompareTo(b.Priority));
        }
        
        
        public static void RegisterFeedback(IUnitEffectFeedback feedback)
        {
            _feedbacks.Add(feedback);
            _feedbacks.Sort((a, b) => a.Priority.CompareTo(b.Priority));
        }
    }
}
