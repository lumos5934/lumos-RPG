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

        
        public void Add(int id, StatModifier modifier)
        {
            var stat = Get(id);
            if (stat == null)
                return;
            
            stat.Add(modifier);
        }
        
        
        public void Remove(int id, StatModifier modifier)
        {
            var stat = Get(id);
            if (stat == null)
                return;
            
            stat.Remove(modifier);
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
