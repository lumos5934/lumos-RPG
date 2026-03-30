namespace LumosLib.RPG
{
    public interface IUnit
    {
        StatHandler Stats { get; }
        StateHandler States { get; }
        VitalHandler Vitals { get; }
        void OnApplyEffect(IUnitEffectAction action);
    }
}

