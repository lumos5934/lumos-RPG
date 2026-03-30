namespace LumosLib.RPG
{
    public interface IUnitEffectFeedback
    {
        int Order { get; }
        void Apply(IUnitEffectAction action);
    }
}
