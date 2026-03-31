namespace LumosLib.RPG
{
    public interface IUnitEffectModifier
    {
        int Order { get; }
        void Modify(UnitEffectContext context);
    }
}
