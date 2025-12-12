using UnityEngine;

public class DeathState : IMonsterState
{
    private Monster _monster;
    private float _timer = 0f;

    public DeathState(Monster monster)
    {
        _monster = monster;
    }

    public void Enter()
    {
        Debug.Log("Death State");

        // TODO: 죽는 애니메이션 호출
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
        Debug.Log("Exit Death");
    }
}
