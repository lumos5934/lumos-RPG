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


        public void Register(int id, Stat stat)
        {
            _stats.TryAdd(id, stat);
        }


        public void Unregister(int id)
        {
            _stats.Remove(id);
        }

        
        public Stat Get(int id)
        {
            return _stats.GetValueOrDefault(id);
        }


        public float GetValue(int id)
        {
            var stat = Get(id);
            if (stat == null)
                return 0;

            return stat.Value;
        }


        public float GetBaseValue(int id)
        {
            var stat = Get(id);
            if (stat == null)
                return 0;

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
