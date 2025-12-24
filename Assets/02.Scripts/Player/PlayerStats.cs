using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public ConsumableStat Health;
    public ConsumableStat Stamina;
    
    public ValueStat Damage;
    public ValueStat WalkSpeed;
    public ValueStat RunSpeed;
    public ValueStat JumpPower;
    
    public event Action OnStatsChanged;
    
    private void Awake()
    {
        Health.OnValueChanged += HandleStatChanged;
        Stamina.OnValueChanged += HandleStatChanged;
    }
    private void Start()
    {
        Health.Initialize();
        Stamina.Initialize();
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        Health.Regenerate(deltaTime);
        Stamina.Regenerate(deltaTime);
    }
    
    private void HandleStatChanged()
    {
        OnStatsChanged?.Invoke();
    }
}
