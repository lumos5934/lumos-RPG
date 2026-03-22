namespace LumosLib.RPG
{
    public interface IUnitEffectFeedback
    {
        int Priority { get; }
        void Apply(UnitEffectContext ctx);
    }
}
