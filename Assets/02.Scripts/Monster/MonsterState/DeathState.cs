using UnityEngine;
using UnityEngine.AI;

public class DeathState : IMonsterState
{
    private Monster _monster;
    private NavMeshAgent _agent;
    private float _timer = 0f;

    public DeathState(Monster monster)
    {
        _monster = monster;
        _agent = monster.NavMeshAgent;
    }

    public void Enter()
    {
        _agent.isStopped = true;
        
        _monster.Animator.SetTrigger("Death");
    }

    public void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _monster.DeathDelay)
        {
            GameObject.Destroy(_monster.gameObject);
        }
    }

    public void Exit()
    {
        
    }
}
