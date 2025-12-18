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
        Damage damage = new Damage(
            _monster.Stats.Damage.Value,
            _monster.transform.position
        );

        _monster.PlayerHealth.TryTakeDamage(damage);
    }
}
