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

        Debug.Log("Patrol State");
    }

    public void Update()
    {
        Vector3 direction = (_target - _monster.transform.position).normalized;
        _monster.Controller.Move(direction * _monster.Stats.MoveSpeed.Value * Time.deltaTime);

        if (Vector3.Distance(_monster.transform.position, _target) < _monster.PatrolPointReach)
        {
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
        Debug.Log("Exit Patrol");
    }
}
