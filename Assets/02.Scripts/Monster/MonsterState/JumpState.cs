using UnityEngine;
using UnityEngine.AI;

public class JumpState : IMonsterState
{
    private Monster _monster;
    private NavMeshAgent _agent;
    private Vector3 _start;
    private Vector3 _end;
    
    public JumpState(Monster monster, Vector3 start, Vector3 end)
    {
        _monster = monster;
        _agent = monster.NavMeshAgent;
        _start = start;
        _end = end;
    }
    
    public void Enter()
    {
        _agent.isStopped = true;
        _agent.ResetPath();
    }

    public void Update()
    {
        
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }

    private void Jump()
    {
        _agent.isStopped = false;
    }
}
