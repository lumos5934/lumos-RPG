using System;
using System.Collections.Generic;
using UnityEngine;

namespace LLib.RPG
{
    public sealed class UnitEffectContext
    {
        public IUnit Source;
        public IUnit Target;

        public int HitFlags;
        public float HitDistance;
        public Vector3 HitPosition;
        public Vector3 HitDirection;

       
        private readonly List<UnitEffect> _effects = new();
        private readonly Dictionary<Type, object> _customData = new();
        
        public IReadOnlyList<UnitEffect> Effects => _effects;
        
     
        internal UnitEffectContext() { }
        
        
        public void SetEffects(IEnumerable<UnitEffect> effects)
        {
            ClearEffects();

            foreach (var effect in effects)
            {
                var pooledEffect = UnitEffectPool.Get(); 
                pooledEffect.Copy(effect);
                _effects.Add(pooledEffect);
            }
        }
        
        
        private void ClearEffects()
        {
            foreach (var effect in _effects)
            {
                UnitEffectPool.Return(effect);
            }
            _effects.Clear();
        }
        
        
        public void Reset()
        {
            Source = null;
            Target = null;
            HitFlags = 0;
            HitPosition = default;
            HitDirection = default;
            HitDistance = 0f;
            _customData.Clear();
            ClearEffects();
        }
        
        
        public void SetData<T>(T data) => _customData[typeof(T)] = data;
        public T GetData<T>() => _customData.TryGetValue(typeof(T), out var v) ? (T)v : default;
        public bool HasData<T>() => _customData.ContainsKey(typeof(T));
    }
}



