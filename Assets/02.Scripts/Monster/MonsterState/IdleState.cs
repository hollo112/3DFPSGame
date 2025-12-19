using UnityEngine;

public class IdleState : IMonsterState
{
    private IMonsterContext _monster;
    private float _timer = 0f;
    
    public IdleState(IMonsterContext monster)
    {
        _monster = monster;
    }

    public void Enter()
    {
        _timer = 0f;
        
        _monster.Animator.SetTrigger("Idle");
    }

    public void Update()
    {
        _timer += Time.deltaTime;

        if (Vector3.Distance(_monster.Transform.position, _monster.Player.transform.position) <= _monster.DetectDistance)
        {
            _monster.ChangeState(new TraceState(_monster));
            return;
        }

        if (_timer >= _monster.PatrolInterval)
        {
            _monster.ChangeState(new PatrolState(_monster));
        }
    }

    public void Exit()
    {
        
    }
}
