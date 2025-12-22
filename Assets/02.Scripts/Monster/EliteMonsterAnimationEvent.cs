using UnityEngine;

public class EliteMonsterAnimationEvent : MonoBehaviour
{
    private Monster _monster;
    [SerializeField] private float _attackRadius = 5f;
    private void Awake()
    {
        _monster = GetComponentInParent<Monster>();
    }

    public void Attack()
    {
        if (_monster.Player == null) return;

        float distance = Vector3.Distance(_monster.transform.position, _monster.Player.transform.position);

        if (distance > _attackRadius) return;

        Damage damage = new Damage
        {
            Value = _monster.Stats.Damage.Value,
            AttackerPosition = _monster.transform.position
        };

        _monster.PlayerHealth.TryTakeDamage(damage);
    }
}
