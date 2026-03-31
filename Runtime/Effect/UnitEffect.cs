using System.Collections.Generic;

namespace LumosLib.RPG
{
    [System.Serializable]
    public class UnitEffect
    {
        public int TargetVitalID;      //Health ..
        public int TargetStatID;      //Health ..
        
        
        public int MethodFlags;     //Direct, Dot ...
        public int AttributeFlags;    //Normal, Fire ...
        public bool IsHarmful;
        
        
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
                return IsHarmful ? value * -1f : value;
            }
        }
        
        
        public void Copy(UnitEffect origin)
        {
            this.MethodFlags =  origin.MethodFlags;
            this.AttributeFlags = origin.AttributeFlags;
            this.TargetVitalID = origin.TargetVitalID;
            this.TargetStatID = origin.TargetStatID;
            this.IsHarmful = origin.IsHarmful;
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
            TargetVitalID = 0;
            TargetStatID = 0;
            IsHarmful = false;
            FinalMultiplier = 1f;
            BaseValue = 0;
            Duration = 0;
            TickInterval = 0;
            Factors.Clear();
        }
      
    }
}
