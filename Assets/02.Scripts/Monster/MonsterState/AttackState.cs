using UnityEngine;
using UnityEngine.AI;

public class AttackState : IMonsterState
{
    private IMonsterContext _monster;
    private float _attackTimer = 0f;
    
    public AttackState(IMonsterContext monster)
    {
        _monster = monster;
    }

    public void Enter()
    {
        _attackTimer = 0f;
        _monster.NavMeshAgent.isStopped = true;
        _monster.NavMeshAgent.ResetPath();
        _monster.Animator.SetTrigger("AttackIdle");
    }

    public void Update()
    {
        Vector3 monsterPosition = _monster.Transform.position;
        Vector3 playerPos = _monster.Player.transform.position;

        float distance = Vector3.Distance(monsterPosition, playerPos);

        if (distance > _monster.AttackDistance)
        {
            _monster.ChangeState(new TraceState(_monster));
            return;
        }

        _attackTimer += Time.deltaTime;

        if (_attackTimer >= _monster.Stats.AttackSpeed.Value)
        {
            _attackTimer = 0f;

            _monster.Animator.SetTrigger("Attack");
        }
    }

    public void Exit()
    {
        _monster.NavMeshAgent.isStopped = false;
    }
}
