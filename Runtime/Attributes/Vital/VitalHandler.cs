using System.Collections.Generic;

namespace LumosLib.RPG
{
    public class VitalHandler
    {
        private Dictionary<int, Vital> _vitals;
        
        
        public VitalHandler()
        {
            _vitals = new();
        }


        public Vital Get(int id)
        {
            return _vitals.GetValueOrDefault(id);
        }

        
        public void Register(int id, Vital vital)
        {
            _vitals.TryAdd(id, vital);
        }
        
        
        public void Unregister(int id)
        {
            var contains = Get(id);
            if (contains != null)
            {
                contains.Dispose();
                _vitals.Remove(id);
            }
        }
        
        
        public void Apply(int id, float amount)
        {
            var vital = Get(id);
            vital?.Apply(amount); 
        }


        public void SetEmpty(int id)
        {
            var vital = Get(id);
            vital?.SetEmpty();
        }


        public void SetFull(int id)
        {
            var vital = Get(id);
            vital?.SetFull();
        }
    }
}
