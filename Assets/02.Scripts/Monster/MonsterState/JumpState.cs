using UnityEngine;
using UnityEngine.AI;

public class JumpState : IMonsterState
{
    private Monster _monster;
    private NavMeshAgent _agent;
    private Vector3 _start;
    private Vector3 _end;
    
    private float _duration = 0.6f;     // 점프 시간
    private float _jumpHeight = 2.5f;   // 점프 높이
    private float _timer;
    public JumpState(Monster monster, Vector3 start, Vector3 end)
    {
        _monster = monster;
        _agent = monster.NavMeshAgent;
        _start = start;
        _end = end;
    }
    
    public void Enter()
    {
        _timer = 0f;
        _agent.isStopped = true;
        _agent.ResetPath();
        
        Animator animator = _monster.Animator; 
        animator.SetTrigger("Jump");
    }

    public void Update()
    {
        _timer += Time.deltaTime;
        float time = Mathf.Clamp01(_timer / _duration);

        Vector3 position = Vector3.Lerp(_start, _end, time);

        float jumpY = _jumpHeight * Mathf.Sin(Mathf.PI * time);
        position.y += jumpY;

        _monster.transform.position = position;

        Vector3 direction = (_end - position);
        direction.y = 0f;
        if (direction.sqrMagnitude > 0.001f)
            _monster.RotateToward(direction);

        if (time >= 1f)
        {
            _agent.updatePosition = true;
            _agent.updateRotation = true;

            _agent.CompleteOffMeshLink(); 
            _monster.ChangeState(new TraceState(_monster));
        }
    }

    public void Exit()
    {
        _agent.isStopped = false;
    }
}
