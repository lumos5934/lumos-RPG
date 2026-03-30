namespace LumosLib.RPG
{
    public struct UnitEffect
    {
        public int MethodFlags;     //Direct, Dot ...
        public int AttributeFlags;    //Normal, Fire ...
        public int VitalTypeID;      //Health ..
        public int SourceID;        //Weapon ..
        public bool IsHarmful;
        public float Value;
    }
}
