using System.Collections.Generic;
using UnityEngine;

namespace LumosLib.RPG
{
    public class UnitEffectContext
    {
        public IUnit Source;
        public IUnit Target;
        public List<UnitEffect> Effects = new();
        public Vector3 HitPoint; 
        public Vector3 HitDirection;
        public int HitFlags;     //Crit, Evade, Block ...
        
        
        internal UnitEffectContext()
        {
        }
        
        
        public void Reset()
        {
            Source = null; 
            Target = null;
            Effects.Clear();
            
            HitPoint = Vector3.zero;
            HitDirection = Vector3.zero;
            HitFlags = 0;
        }
    }
}
