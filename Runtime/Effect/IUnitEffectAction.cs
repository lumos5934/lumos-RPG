using System.Collections.Generic;

namespace LumosLib.RPG
{
    public interface IUnitEffectAction
    {
        IUnit Source { get; }
        IUnit Target { get; }
        IReadOnlyList<UnitEffect> Effects { get; }
        IReadOnlyList<UnitEffectCost> Costs { get; }
    }
}

