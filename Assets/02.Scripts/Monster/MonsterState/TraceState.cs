using UnityEngine;

public class TraceState : IMonsterState
{
    private Monster _monster;

    public TraceState(Monster monster)
    {
        _monster = monster;
    }

    public void Enter()
    {
        Debug.Log("Trace State");
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

        Vector3 direction = (playerPosition - monsterPosition).normalized;
        _monster.Controller.Move(direction * _monster.Stats.MoveSpeed.Value * Time.deltaTime);
    }

    public void Exit()
    {
        Debug.Log("Exit Trace");
    }
}
