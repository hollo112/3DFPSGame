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
        Debug.Log("Comeback State");
    }

    public void Update()
    {
        Vector3 monsterPosition = _monster.transform.position;
        Vector3 originPosition = _monster.OriginPosition;
        Vector3 playerPosirion = _monster.Player.transform.position;

        float toOrigin = Vector3.Distance(monsterPosition, originPosition);
        float toPlayer = Vector3.Distance(monsterPosition, playerPosirion);

        if (toPlayer <= _monster.DetectDistance)
        {
            _monster.ChangeState(new TraceState(_monster));
            return;
        }

        Vector3 direction = (originPosition - monsterPosition).normalized;
        _monster.Controller.Move(direction * _monster.Stats.MoveSpeed.Value * Time.deltaTime);

        if (toOrigin <= 0.1f)
        {
            _monster.ChangeState(new PatrolState(_monster));
        }
    }

    public void Exit()
    {
        Debug.Log("Exit Comeback");
    }
}
