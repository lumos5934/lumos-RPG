using System;
using UnityEngine;

namespace LLib.RPG
{
    public class StatResource
    {
        private readonly int _id;
        private readonly Stat _refStat;
        
        private float _current;

        public event Action<float, float> OnValueChanged;
        public event Action OnEmpty;

        public int ID => _id;
        public float Current => _current;
        public float Max => _refStat.Value;
        public float Ratio => Mathf.Clamp01(_current / Max);

        
        public StatResource(Stat refStat)
        {
            _id = refStat.ID;
            
            _refStat = refStat;
            _refStat.OnValueChanged += OnRefStatChanged;
            
            _current = refStat.Value;
        }


        public void Dispose()
        {
            if (_refStat != null)
            {
                _refStat.OnValueChanged -= OnRefStatChanged;
            }
        }
        
        
        public void Apply(float amount)
        {
            if (amount == 0) 
                return;

            float previous = _current;
            _current = Mathf.Clamp(_current + amount, 0, Max);

            if (!Mathf.Approximately(previous, _current))
            {
                OnValueChanged?.Invoke(_current, Max);
                
                if (_current <= 0)
                {
                    OnEmpty?.Invoke();
                }
            }
        }


        public void SetEmpty()
        {
            _current = 0;
            
            OnValueChanged?.Invoke(_current, Max);
            OnEmpty?.Invoke();
        }
        

        public void SetFull()
        {
            _current = Max;
            
            OnValueChanged?.Invoke(_current, Max);
        }


        protected virtual void OnRefStatChanged(float max)
        {
            _current = Math.Min(_current, max);

            OnValueChanged?.Invoke(_current, max);
        }
    }
}
