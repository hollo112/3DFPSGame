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

    public event Action OnValueChanged;
        
    public void Initialize()
    {
        _value = _maxValue;
        Notify();
    }
    
    public void Regenerate(float time)
    {
        if (_regenValue <= 0f) return;

        float before = _value;
        _value += _regenValue * time;
        _value = Mathf.Min(_value, _maxValue);

        if (!Mathf.Approximately(before, _value))
            Notify();
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
        Notify();
    }

    public void IncreaseMax(float amount)
    {
        _maxValue += amount;
        Notify();
    }
    
    public float ConsumeClamped(float amount)
    {
        float damageApplied = Mathf.Min(_value, amount);
        _value -= damageApplied;
        Notify();
        return damageApplied;
    }
    
    public void Increase(float amount)
    {
        _value += amount;
        Notify();
    }

    public void DecreaseMax(float amount)
    {
        _maxValue -= amount;
        Notify();
    }

    public void Decrease(float amount)
    {
        _value -= amount;
        Notify();
    }

    public void SetMax(float amount)
    {
        _maxValue = amount;
        Notify();
    }
    
    public void SetValue(float value)
    {
        _value = value;
        Notify();
    }

    private void Notify()
    {
        OnValueChanged?.Invoke();
    }
}
