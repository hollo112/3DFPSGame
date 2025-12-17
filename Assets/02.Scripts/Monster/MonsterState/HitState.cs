using UnityEngine;
using UnityEngine.AI;

public class HitState : IMonsterState
{
    private Monster _monster;
    private IMonsterState _previousState;
    private Damage _damage;
    private float _timer = 0f;
    private Vector3 _knockbackVelocity;
    private NavMeshAgent _agent;
    public HitState(Monster monster, IMonsterState previousState, Damage damage)
    {
        _monster = monster;
        _agent = _monster.NavMeshAgent;
        _previousState = previousState;
        _damage = damage;
    }

    public void Enter()
    {
        Vector3 direction = (_monster.transform.position - _damage.AttackerPosition).normalized;
        direction.y = 0;

        _knockbackVelocity = direction * _damage.KnockbackForce;
        
        _agent.isStopped = true;
        _agent.ResetPath();
    }

    public void Update()
    {
        _timer += Time.deltaTime;

        _monster.MoveRaw(_knockbackVelocity);

        _knockbackVelocity = Vector3.MoveTowards(_knockbackVelocity, Vector3.zero, _monster.KnockbackDrag * Time.deltaTime);

        if (_timer >= _monster.HitDuration)
        {
            _monster.ChangeState(_previousState);
        }
    }

    public void Exit()
    {
        _agent.isStopped = false;
    }
}
