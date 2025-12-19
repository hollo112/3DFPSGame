using UnityEngine;

public class PatrolState : IMonsterState
{
    private IMonsterContext _monster;
    private Vector3 _target;

    public PatrolState(IMonsterContext monster)
    {
        _monster = monster;
    }

    public void Enter()
    {
        float patrolRadius = _monster.PatrolRadius;
        _target = _monster.OriginPosition + new Vector3(Random.Range(-patrolRadius, patrolRadius), 0, Random.Range(-patrolRadius, patrolRadius));
       
        _monster.Animator.ResetTrigger("Idle");
        _monster.Animator.SetTrigger("Patrol");
    }

    public void Update()
    {
        _monster.MoveTo(_target);

        if (Vector3.Distance(_monster.Transform.position, _target) < _monster.PointReach)
        {
            _monster.ChangeState(new IdleState(_monster));
            return;
        }

        if (Vector3.Distance(_monster.Transform.position, _monster.Player.transform.position) <= _monster.DetectDistance)
        {
            _monster.ChangeState(new TraceState(_monster));
        }
    }

    public void Exit()
    {
        
    }
}
