using UnityEngine;
using UnityEngine.AI;

public class TraceState : IMonsterState
{
    private Monster _monster;
    private NavMeshAgent _agent;
    
    public TraceState(Monster monster)
    {
        _monster = monster;
        _agent = monster.NavMeshAgent;
    }

    public void Enter()
    {
        
    }

    public void Update()
    {
        Vector3 monsterPosition = _monster.transform.position;
        Vector3 playerPosition = _monster.Player.transform.position;

        float distance = Vector3.Distance(monsterPosition, playerPosition);

        if (distance > _monster.DetectDistance)
        {
            _monster.ChangeState(new ComebackState(_monster));
            return;
        }

        if (distance <= _monster.AttackDistance)
        {
            _monster.ChangeState(new AttackState(_monster));
            return;
        }
        
        _monster.MoveTo(playerPosition);
    }

    public static bool IsOnJumpSurface(Vector3 position, float sampleRadius = 0.5f)
    {
        Vector3 samplePos = position + Vector3.down * 1.0f;
        if (NavMesh.SamplePosition(
                samplePos,
                out NavMeshHit hit,
                sampleRadius,
                NavMesh.AllAreas))
        {
            int jumpArea = NavMesh.GetAreaFromName("Jump");
            Debug.Log(jumpArea);
            return (hit.mask & (1 << jumpArea)) != 0;
        }

        return false;
    }

    public void Exit()
    {
        
    }
}
