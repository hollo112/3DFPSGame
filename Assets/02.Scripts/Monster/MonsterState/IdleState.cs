using UnityEngine;

public class IdleState : IMonsterState
{
    private Monster _monster;
    private float _timer = 0f;
    
    public IdleState(Monster monster)
    {
        _monster = monster;
    }

    public void Enter()
    {
        _timer = 0f;
        Debug.Log("Idle State");
    }

    public void Update()
    {
        _timer += Time.deltaTime;

        if (Vector3.Distance(_monster.transform.position, _monster.Player.transform.position) <= _monster.DetectDistance)
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
        Debug.Log("Exit Idle");
    }
}
