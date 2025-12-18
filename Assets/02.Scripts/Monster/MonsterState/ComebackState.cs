using UnityEngine;

public class ComebackState : IMonsterState
{
    private Monster _monster;

    public ComebackState(Monster monster)
    {
        _monster = monster;
    }

    public void Enter()
    {
        _monster.Animator.SetTrigger("Comeback");
    }

    public void Update()
    {
        Vector3 monsterPosition = _monster.transform.position;
        Vector3 originPosition = _monster.OriginPosition;
        Vector3 playerPosition = _monster.Player.transform.position;

        float toOrigin = Vector3.Distance(monsterPosition, originPosition);
        float toPlayer = Vector3.Distance(monsterPosition, playerPosition);

        if (toPlayer <= _monster.DetectDistance)
        {
            _monster.ChangeState(new TraceState(_monster));
            return;
        }

        _monster.MoveTo(originPosition);
        
        if (toOrigin <= _monster.PointReach)
        {
            _monster.ChangeState(new PatrolState(_monster));
        }
    }

    public void Exit()
    {
        
    }
}
