using System.Collections.Generic;

namespace LumosLib.RPG
{
    public static class UnitEffectSystem
    {
        private static List<IUnitEffectModifier> _modifiers = new();
        private static List<IUnitEffectFeedback> _feedbacks = new();
        
        
        public static void Apply(IUnitEffectAction action)
        {
            if (action == null || 
                action.Target == null || 
                action.Source == null)
            {
                DebugUtil.LogWarning($"{action?.GetType().Name} : Missing Target or Source", "UnitEffectSystem");
                return;
            }
            
            
            foreach (var modifier in _modifiers)
            {
                modifier.Modify(action);
            }


            PayCosts(action);
            
            ApplyEffect(action);
            
            
            foreach (var feedback in _feedbacks)
            {
                feedback.Apply(action);
            }
        }

        
        public static void AddModifier(IUnitEffectModifier modifier)
        {
            _modifiers.Add(modifier);
            _modifiers.Sort((a, b) => a.Order.CompareTo(b.Order));
        }
        
        
        public static void AddFeedback(IUnitEffectFeedback feedback)
        {
            _feedbacks.Add(feedback);
            _feedbacks.Sort((a, b) => a.Order.CompareTo(b.Order));
        }
        
        
        private static void PayCosts(IUnitEffectAction action)
        {
            var sourceVitals = action.Source.Vitals;

            if (action.Costs == null)
                return;

            foreach (var cost in action.Costs)
            {
                var vital = sourceVitals.Get(cost.VitalTypeID);
                vital?.Apply(-cost.Value);
            }
        }


        private static void ApplyEffect(IUnitEffectAction action)
        {
            var targetVitals = action.Target.Vitals;
            
            var effects = action.Effects;
            if (effects == null)
                return;
            
            for (int i = 0; i < effects.Count; i++)
            {
                var effect = effects[i];
                
                var targetVital = targetVitals.Get(effect.VitalTypeID);
                if (targetVital != null)
                {
                    var value = effect.Value;

                    if (effect.IsHarmful)
                    {
                        value *= -1f;
                    }
                    
                    targetVital.Apply(value);
                }
            }
            
            action.Target.OnApplyEffect(action);
        }
    }
}
