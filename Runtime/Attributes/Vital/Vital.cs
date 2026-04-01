using System;
using UnityEngine;

namespace LumosLib.RPG
{
    public class Vital
    {
        private int _id;
        private float _current;
        private readonly Stat _maxStat;

        public event Action<float, float> OnValueChanged;
        public event Action OnEmpty;

        public int ID => _id;
        public float Current => _current;
        public float Max => _maxStat.Value;
        public float Ratio => Mathf.Clamp01(_current / Max);

        
        public Vital(int id, Stat maxStat)
        {
            _maxStat = maxStat;
            _maxStat.OnValueChanged += OnMaxStatChanged;
            
            _current = maxStat.Value;
        }


        public void Dispose()
        {
            if (_maxStat != null)
            {
                _maxStat.OnValueChanged -= OnMaxStatChanged;
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


        protected virtual void OnMaxStatChanged(float max)
        {
            _current = Math.Min(_current, max);

            OnValueChanged?.Invoke(_current, max);
        }
    }
}
