using System.Collections.Generic;

namespace LLib.RPG
{
    [System.Serializable]
    public class UnitEffect
    {
        public int TargetStatID;      //Health ..
        public int TargetResourceID;      //Health ..
        
        
        public int MethodFlags;     //Direct, Dot ...
        public int AttributeFlags;    //Normal, Fire ...
        public bool IsPositive;
        
        
        public float BaseValue;
        public float AdditionalValue;
        public float FinalMultiplier = 1f; 
       
        
        public float Duration;
        public float TickInterval;

        
        public List<EffectFactor> Factors = new();
        
        
        public float FinalValue
        {
            get
            {
                float value = (BaseValue + AdditionalValue) * FinalMultiplier;
                return IsPositive ? value : value * -1f;
            }
        }
        
        
        public void Copy(UnitEffect origin)
        {
            this.MethodFlags =  origin.MethodFlags;
            this.AttributeFlags = origin.AttributeFlags;
            this.TargetResourceID = origin.TargetResourceID;
            this.TargetStatID = origin.TargetStatID;    
            this.IsPositive = origin.IsPositive;
            this.FinalMultiplier = origin.FinalMultiplier;
            this.BaseValue = origin.BaseValue;
            this.Duration = origin.Duration;
            this.TickInterval = origin.TickInterval;
            
            this.Factors.Clear();
            this.Factors.AddRange(origin.Factors);
        }
        
        
        public void Reset()
        {
            MethodFlags = 0;
            AttributeFlags = 0;
            TargetResourceID = 0;
            TargetStatID = 0;
            IsPositive = false;
            FinalMultiplier = 1f;
            BaseValue = 0;
            Duration = 0;
            TickInterval = 0;
            Factors.Clear();
        }
      
    }
}
