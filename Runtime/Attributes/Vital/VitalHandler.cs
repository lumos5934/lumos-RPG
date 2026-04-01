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

        
        public void Register(Vital vital)
        {
            _vitals.TryAdd(vital.ID, vital);
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
        
        
        public Vital Get(int id)
        {
            return _vitals.GetValueOrDefault(id);
        }


        public float GetCurrent(int id)
        {
            var target = Get(id);
            if (target == null)
                return 0;
            
            return target.Current;
        }
        
        
        public float GetMax(int id)
        {
            var target = Get(id);
            if (target == null)
                return 0;
            
            return target.Max;
        }
        
        
        public float GetRatio(int id)
        {
            var target = Get(id);
            if (target == null)
                return 0;
            
            return target.Ratio;
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
