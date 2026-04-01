namespace LLib.RPG
{
    [System.Serializable]
    public struct EffectFactor
    {
        public readonly bool IsRefSource;
        public readonly int RefID;
        public readonly float Value;

        public EffectFactor(bool isRefSource, int refID, float value)
        {
            IsRefSource = isRefSource;
            RefID = refID;
            Value = value;
        }
    }
}

