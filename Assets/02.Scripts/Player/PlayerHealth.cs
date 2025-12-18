using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    private PlayerStats _stats;
    private Animator _animator;
    public event Action<Damage> OnDamaged;

    private void Awake()
    {
        _stats = GetComponent<PlayerStats>();
        _animator = GetComponentInChildren<Animator>();
    }

    public bool TryTakeDamage(Damage damage)
    {
        // Todo. 플레이어 죽었을때 혹은 데미지를 입으면 안될때 false
        _stats.Health.ConsumeClamped(damage.Value);
        
        OnDamaged?.Invoke(damage);
            
        if (_stats.Health.Value <= 0)
        {
            _animator.applyRootMotion = true;
            _animator.SetTrigger("Death");
            GameManager.Instance.GameOver();
        }
        
        return true;
    }
}
