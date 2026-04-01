using System.Collections.Generic;
using UnityEngine;

namespace LLib.RPG
{
    public class BuffManager : MonoBehaviour
    {
        private readonly List<BaseBuff> _allBuffs = new();
        private readonly Dictionary<IUnit, Dictionary<int, BaseBuff>> _targetBuffMap = new();


        private void Awake()
        {
            Services.Register(this);
        }

        public void Add(IUnit target, BaseBuff newBuff)
        {
            if (_targetBuffMap.TryGetValue(target, out var activeBuffs))
            {
                if (activeBuffs.TryGetValue(newBuff.ID, out var containBuff))
                {
                    containBuff.AddStack();
                    return;
                }
            }

            newBuff.Apply(target);
            _allBuffs.Add(newBuff);

            if (!_targetBuffMap.ContainsKey(target))
            {
                _targetBuffMap[target] = new Dictionary<int, BaseBuff>();
            }
                
            _targetBuffMap[target][newBuff.ID] = newBuff;
        }

        
        private void Update()
        {
            float deltaTime = Time.deltaTime;
            
            for (int i = _allBuffs.Count - 1; i >= 0; i--)
            {
                var buff = _allBuffs[i];
                
                if (buff.Target == null || buff.Timer <= 0) 
                {
                    RemoveBuff(i, buff);
                    continue;
                }

                buff.Update(deltaTime);
            }
        }

        
        private void RemoveBuff(int index, BaseBuff buff)
        {
            if (_targetBuffMap.TryGetValue(buff.Target, out var activeBuffs))
            {
                activeBuffs.Remove(buff.ID);
                
                if (activeBuffs.Count == 0)
                {
                    _targetBuffMap.Remove(buff.Target);
                }
            }

            buff.Remove();
            
            _allBuffs.RemoveAt(index);
        }
    }
}
