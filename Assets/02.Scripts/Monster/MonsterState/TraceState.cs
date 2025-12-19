using UnityEngine;
using UnityEngine.AI;

public class TraceState : IMonsterState
{
    private IMonsterContext _monster;
    
    public TraceState(IMonsterContext monster)
    {
        _monster = monster;
    }

    public void Enter()
    {
        _monster.Animator.SetTrigger("Trace");
        _monster.NavMeshAgent.speed = _monster.Stats.RunSpeed.Value;
    }

    public void Update()
    {
        Vector3 monsterPosition = _monster.Transform.position;
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

        if (_monster.NavMeshAgent.isOnOffMeshLink)
        {
            OffMeshLinkData data = _monster.NavMeshAgent.currentOffMeshLinkData;

            Vector3 start = _monster.Transform.position;
            Vector3 end   = data.endPos;
            end.y += _monster.NavMeshAgent.baseOffset;
            _monster.ChangeState(new JumpState(_monster, start, end));
        }
    }

    public void Exit()
    {
        _monster.NavMeshAgent.speed = _monster.Stats.WalkSpeed.Value;
    }
}
