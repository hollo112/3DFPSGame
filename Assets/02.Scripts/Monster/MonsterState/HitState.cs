using UnityEngine;

public class HitState : IMonsterState
{
    private Monster _monster;
    private IMonsterState _previousState;
    private Damage _damage;
    private float _duration = 0.25f;
    private float _timer = 0f;
    private Vector3 _knockbackVelocity;

    public HitState(Monster monster, IMonsterState previousState, Damage damage)
    {
        _monster = monster;
        _previousState = previousState;
        _damage = damage;
    }

    public void Enter()
    {
        Debug.Log("Hit State");

        Vector3 direction = (_monster.transform.position - _damage.AttackerPosition).normalized;
        direction.y = 0;

        _knockbackVelocity = direction * _monster.KnockbackForce;
    }

    public void Update()
    {
        _timer += Time.deltaTime;

        _monster.Controller.Move(_knockbackVelocity * Time.deltaTime);

        _knockbackVelocity = Vector3.Lerp(_knockbackVelocity, Vector3.zero, _monster.KnockbackDrag * Time.deltaTime);

        if (_timer >= _duration)
        {
            _monster.ChangeState(_previousState);
        }
    }

    public void Exit()
    {
        Debug.Log("Exit Hit");
    }
}
