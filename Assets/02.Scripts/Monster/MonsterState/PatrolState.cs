using UnityEngine;

public class PatrolState : IMonsterState
{
    private Monster _monster;
    private Vector3 _target;

    public PatrolState(Monster monster)
    {
        _monster = monster;
    }

    public void Enter()
    {
        float patrolRadius = _monster.PatrolRadius;
        _target = _monster.OriginPosition + new Vector3(Random.Range(-patrolRadius, patrolRadius), 0, Random.Range(-patrolRadius, patrolRadius));
        Animator animator = _monster.Animator;
        animator.ResetTrigger("Idle");
        animator.SetTrigger("Patrol");
    }

    public void Update()
    {
        _monster.MoveTo(_target);

        if (Vector3.Distance(_monster.transform.position, _target) < _monster.PointReach)
        {
            Debug.Log("돌아옴");
            _monster.ChangeState(new IdleState(_monster));
            return;
        }

        if (Vector3.Distance(_monster.transform.position, _monster.Player.transform.position) <= _monster.DetectDistance)
        {
            _monster.ChangeState(new TraceState(_monster));
        }
    }

    public void Exit()
    {
        
    }
}
