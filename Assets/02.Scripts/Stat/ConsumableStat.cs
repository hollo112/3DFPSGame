using System;
using UnityEngine;

[Serializable]
public class ConsumableStat 
{
    [SerializeField] private float _maxValue;
    public float MaxValue => _maxValue;
    [SerializeField] private float _value;
    public float Value => _value;
    [SerializeField] private float _regenValue;

    public void Initialize()
    {
        _value = _maxValue;    
    }
    
    public void Regenerate(float time)
    {
        _value += _regenValue * time;

        if (_value > _maxValue)
        {
            _value = _maxValue;
        }
    }

    public bool TryConsume(float amount)
    {
        if(_value < amount) return false;
        
        Consume(amount);
        
        return true;
    }
    
    public void Consume(float amount)
    {
        _value -= amount;
    }

    public void IncreaseMax(float amount)
    {
        _maxValue += amount;
    }
    
    public float ConsumeClamped(float amount)
    {
        float damageApplied = Mathf.Min(_value, amount);
        _value -= damageApplied;
        return damageApplied;
    }
    
    public void Increase(float amount)
    {
        _value += amount;
    }

    public void DecreaseMax(float amount)
    {
        _maxValue -= amount;
    }

    public void Decrease(float amount)
    {
        _value -= amount;
    }

    public void SetMax(float amount)
    {
        _maxValue = amount;
    }
    
    public void SetValue(float value)
    {
        _value = value;
    }
}
