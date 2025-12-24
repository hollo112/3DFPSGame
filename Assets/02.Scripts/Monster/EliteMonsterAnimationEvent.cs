using UnityEngine;

public class EliteMonsterAnimationEvent : MonoBehaviour
{
    private Monster _monster;
    [SerializeField] private GameObject _indicator;
    [SerializeField] private float _attackRadius = 5f;
    private void Awake()
    {
        _monster = GetComponentInParent<Monster>();
    }
    
    
    private void OnEnable()
    {
        _indicator.SetActive(false);
        _monster.OnAttackWithRadius += HandleAttackRadius;
    }

    private void OnDisable()
    {
        _monster.OnAttackWithRadius -= HandleAttackRadius;
    }


    public void Attack()
    {
        _indicator.SetActive(false);
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
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Transform center = transform.parent != null 
            ? transform.parent 
            : transform;

        Gizmos.DrawWireSphere(center.position, 1f);
    }
    
    private void HandleAttackRadius(float radius)
    {
        _indicator.SetActive(true);
    }
}
