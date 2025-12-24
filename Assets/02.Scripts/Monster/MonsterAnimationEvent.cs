using System;
using UnityEngine;

public class MonsterAnimationEvent : MonoBehaviour
{
    private Monster _monster;
    
    private void Awake()
    {
        _monster = GetComponentInParent<Monster>();
    }

    public void Attack()
    {
        Damage damage = new Damage
        { 
            Value = _monster.Stats.Damage.Value,
            AttackerPosition = _monster.transform.position
        };

        _monster.PlayerHealth.TryTakeDamage(damage);
    }
}
