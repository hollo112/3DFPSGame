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
        _monster.Animator.SetTrigger("Trace");
        _agent.speed = _monster.Stats.RunSpeed.Value;
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

        if (_agent.isOnOffMeshLink)
        {
            OffMeshLinkData data = _agent.currentOffMeshLinkData;

            Vector3 start = _monster.transform.position;
            Vector3 end   = data.endPos;
            end.y += _agent.baseOffset;
            _monster.ChangeState(new JumpState(_monster, start, end));
        }
    }

    public void Exit()
    {
        _agent.speed = _monster.Stats.WalkSpeed.Value;
    }
}
