namespace LLib.RPG
{
    public interface IUnit
    {
        StatHandler Stats { get; }
        StatResourceHandler Resources { get; }
        StateHandler States { get; }
        void OnApplyEffect(UnitEffectContext context);
    }
}

