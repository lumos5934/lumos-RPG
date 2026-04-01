using System;
using System.Collections.Generic;

namespace LumosLib.RPG
{
	public class Stat
	{
		private int _id;
		private bool _isDirty = true;
		private float _baseValue;
		private float _value;
		private object _sourceToRemove;
		
		private readonly List<StatModifier> _modifiers;
		private readonly Comparison<StatModifier> _comparison;
		private readonly Predicate<StatModifier> _predicate;

	    public event Action<float> OnValueChanged;
		
		public int ID => _id;
		public IReadOnlyList<StatModifier> Modifiers => _modifiers;
		public float BaseValue => _baseValue;
		public float Value
		{
			get
			{
				if (_isDirty)
				{
					_value = CalculateValue();
					_isDirty = false;
				}
				return _value;
			}
		}

		
		public Stat(int id)
		{
			_id = id;
			
			_modifiers = new List<StatModifier>();
			_comparison = GetModSortOrder;
			_predicate = modifier => modifier.Source == _sourceToRemove;
		}

		
		public Stat(int id, float baseValue) : this(id)
		{
			SetBaseValue(baseValue);
		}


		public void SetBaseValue(float baseValue)
		{
			_baseValue = baseValue;
			_isDirty = true;

			OnValueChanged?.Invoke(Value);
		}
		
		
		public void AddModifier(StatModifier mod)
		{
			_modifiers.Add(mod);
			_isDirty = true;
			
			OnValueChanged?.Invoke(Value);
		}

		
		public bool RemoveModifier(StatModifier mod)
		{
			if (_modifiers.Remove(mod))
			{
				_isDirty = true;
				
				OnValueChanged?.Invoke(Value);
				
				return true;
			}
			
			return false;
		}

		
		public bool RemoveAllFromSource(object source)
		{
			_sourceToRemove = source;
			
			int numRemovals = _modifiers.RemoveAll(_predicate);
			
			_sourceToRemove = null;

			if (numRemovals > 0)
			{
				_isDirty = true;
				return true;
			}
			
			return false;
		}

		
		private int GetModSortOrder(StatModifier a, StatModifier b)
		{
			if (a.Order < b.Order)
				return -1;
			
			if (a.Order > b.Order)
				return 1;
			
			return 0; 
		}

		
		private float CalculateValue()
		{
			float value = BaseValue;
			float percentAddResult = 0;

			_modifiers.Sort(_comparison);

			for (int i = 0; i < _modifiers.Count; i++)
			{
				StatModifier mod = _modifiers[i];

				switch (mod.Type)
				{
					case StatModType.Flat :
						value += mod.Value;
						break;
					
					case StatModType.PercentAdd :
						percentAddResult += mod.Value;

						if (i + 1 >= _modifiers.Count ||
						    _modifiers[i + 1].Type != StatModType.PercentAdd)
						{
							value *= 1 + percentAddResult;
							percentAddResult = 0;
						}
						break;
					
					case StatModType.PercentMult :
						value *= 1 + mod.Value;
						break;
				}
			}
			
			return (float)Math.Round(value, 4);
		}
	}
}
