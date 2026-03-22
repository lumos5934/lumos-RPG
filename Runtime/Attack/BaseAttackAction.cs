using System.Collections.Generic;

namespace LumosLib.RPG
{
    public abstract class BaseAttackAction
    {
        public string Name { get; }
        public IReadOnlyList<UnitEffect> Effects { get; }
        public IReadOnlyList<UnitEffectCost> Costs { get; }


        public BaseAttackAction(string name, List<UnitEffect> effects, List<UnitEffectCost> costs)
        {
            Name = name;
            Effects = effects;
            Costs = costs;
        }


        public void Attack(IUnit source, IUnit target)
        {
            if (!IsFreeCost(source))
            {
                foreach (var cost in Costs)
                {
                    var curVital = source.Vitals.Get(cost.VitalTypeID);

                    if (curVital == null ||
                        curVital.Current < cost.Value)
                        return;
                    
                    source.Vitals.Apply(cost.VitalTypeID, - cost.Value);
                }
            }
            
            OnAttack(source, target);
        }


        protected abstract void OnAttack(IUnit source, IUnit target);

        protected virtual bool IsFreeCost(IUnit source)
        {
            return false;
        }
    }
}
