using System.Collections.Generic;

namespace LumosLib.RPG
{
    public class StatHandler
    {
        private Dictionary<int, Stat> _stats;
        
        public StatHandler()
        {
            _stats = new();
        }

        
        public Stat Get(int id)
        {
            if (!_stats.TryGetValue(id, out var stat))
            {
                stat = new Stat(); 
                _stats.Add(id, stat);
            }
            
            return stat;
        }


        public float GetValue(int id)
        {
            var stat = Get(id);

            return stat.Value;
        }


        public float GetBaseValue(int id)
        {
            var stat = Get(id);

            return stat.BaseValue;
        }


        public IReadOnlyList<StatModifier> GetModifiers(int id)
        {
            var stat = Get(id);

            return stat?.Modifiers;
        }
        
        
        public void SetBaseValue(int id, float value)
        {
            var stat = Get(id);

            stat?.SetBaseValue(value);
        }
        
        
        public void AddModifier(int id, StatModifier modifier)
        {
            var stat = Get(id);

            stat?.AddModifier(modifier);
        }
        
        
        public void RemoveModifier(int id, StatModifier modifier)
        {
            var stat = Get(id);

            stat?.RemoveModifier(modifier);
        }

        
        public void RemoveAllFromSource(object source)
        {
            foreach (var stat in _stats.Values)
            {
                stat.RemoveAllFromSource(source);
            }
        }
    }
}
