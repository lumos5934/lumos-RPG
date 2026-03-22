namespace LumosLib.RPG
{
    public interface IUnitEffectModifier
    {
        int Priority { get; }
        void Modify(UnitEffectContext ctx);
    }
}
