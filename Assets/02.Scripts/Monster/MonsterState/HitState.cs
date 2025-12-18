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
    private float _hitDuration;
    private float _defaultAnimationDuration = 1.5f;
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
        
        Animator animator = _monster.Animator; 
        animator.SetTrigger("Hit");
        
        _hitDuration = GetHitAnimationDuration(animator);
    }

    public void Update()
    {
        _timer += Time.deltaTime;

        //_monster.MoveRaw(_knockbackVelocity);

        //_knockbackVelocity = Vector3.MoveTowards(_knockbackVelocity, Vector3.zero, _monster.KnockbackDrag * Time.deltaTime);

        if (_timer >= _hitDuration)
        {
            _monster.ChangeState(_previousState);
        }
    }

    public void Exit()
    {
        _agent.isStopped = false;
    }
    
    private float GetHitAnimationDuration(Animator animator)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
    
        if (stateInfo.IsName("Hit"))
        {
            return stateInfo.length;
        }
    
        AnimatorStateInfo nextStateInfo = animator.GetNextAnimatorStateInfo(0);
        if (nextStateInfo.IsName("Hit"))
        {
            return nextStateInfo.length;
        }
        
        return _defaultAnimationDuration;
    }
}
