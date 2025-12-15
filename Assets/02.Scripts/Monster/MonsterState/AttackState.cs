using UnityEngine;

public class AttackState : IMonsterState
{
    private Monster _monster;
    private float _attackTimer = 0f;

    public AttackState(Monster monster)
    {
        _monster = monster;
    }

    public void Enter()
    {
        _attackTimer = 0f;
    }

    public void Update()
    {
        Vector3 monsterPosition = _monster.transform.position;
        Vector3 playerPos = _monster.Player.transform.position;

        float distance = Vector3.Distance(monsterPosition, playerPos);

        if (distance > _monster.AttackDistance)
        {
            _monster.ChangeState(new TraceState(_monster));
            return;
        }
        
        _monster.Move(Vector3.zero);

        _attackTimer += Time.deltaTime;

        if (_attackTimer >= _monster.Stats.AttackSpeed.Value)
        {
            _attackTimer = 0f;

            Damage damage = new Damage(
                _monster.Stats.Damage.Value,
                monsterPosition
            );

            _monster.PlayerHealth.TryTakeDamage(damage);
        }
    }

    public void Exit()
    {
        
    }
}
