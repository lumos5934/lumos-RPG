namespace LumosLib.RPG
{
    [System.Serializable]
    public class UnitEffect
    {
        public int MethodFlags;     //Direct, Dot ...
        public int AttributeFlags;    //Normal, Fire ...
        public int VitalTypeID;      //Health ..
        public int SourceID;        //Weapon ..
        
        public bool IsHarmful;
        public float Multiplier = 1f; 
        public float BaseValue;
        public float Duration;
        public float TickInterval;

        public float FinalValue
        {
            get
            {
                float value = BaseValue * Multiplier;
                return IsHarmful ? value * -1f : value;
            }
        }
        
        
        public void Copy(UnitEffect origin)
        {
            this.MethodFlags =  origin.MethodFlags;
            this.AttributeFlags = origin.AttributeFlags;
            this.VitalTypeID = origin.VitalTypeID;
            this.SourceID = origin.SourceID;
            this.IsHarmful = origin.IsHarmful;
            this.Multiplier = origin.Multiplier;
            this.BaseValue = origin.BaseValue;
            this.Duration = origin.Duration;
            this.TickInterval = origin.TickInterval;
        }
        
        
        public void Reset()
        {
            MethodFlags = 0;
            AttributeFlags = 0;
            VitalTypeID = 0;
            SourceID = 0;
            IsHarmful = false;
            Multiplier = 1f;
            BaseValue = 0;
            Duration = 0;
            TickInterval = 0;
        }
      
    }
}
