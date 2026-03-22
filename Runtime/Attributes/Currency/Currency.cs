using System;

namespace LumosLib.RPG
{
    public class Currency
    {
        public long Value { get; private set; }

        public event Action<long, long> OnChanged;

        
        public Currency()
        {
        }

        
        public Currency(long initialValue) : this()
        {
            Value = Math.Max(0, initialValue);
        }

        
        public void Set(long newValue)
        {
            long previous = Value;
            Value = Math.Max(0, newValue);

            if (previous != Value)
            {
                OnChanged?.Invoke(previous, Value);
            }
        }

        
        public void Add(long amount)
        {
            Set(Value + amount);
        }
        
        
        public bool Consume(long amount)
        {
            if (Value < amount) 
                return false;
            
            Set(Value - amount);
            
            return true;
        }
    }
}
