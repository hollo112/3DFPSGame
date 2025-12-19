using UnityEngine;
using UnityEngine.AI;

public class DeathState : IMonsterState
{
    private IMonsterContext _monster;
    private float _timer = 0f;

    public DeathState(IMonsterContext monster)
    {
        _monster = monster;
    }

    public void Enter()
    {
        _monster.NavMeshAgent.isStopped = true;
        
        _monster.Animator.SetTrigger("Death");
    }

    public void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _monster.DeathDelay)
        {
            GameObject.Destroy(_monster.Transform.gameObject);
        }
    }

    public void Exit()
    {
        
    }
}
