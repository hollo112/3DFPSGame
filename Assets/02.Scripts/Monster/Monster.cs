using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public EMonsterState State = EMonsterState.Idle;
    [SerializeField] private GameObject _player;
    private CharacterController _controller;
    private MonsterStats _stats;

    [SerializeField] private float _detectDistance;
    [SerializeField] private float _attackDistance;
    [SerializeField] private float _knockbackForce = 4f;
    [SerializeField] private float _knockbackDrag = 7f;
    
    private float _attackTimer = 0;
    private Vector3 _originPosition;
    private Vector3 _knockbackVelocity;
    
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _stats = GetComponent<MonsterStats>();
    }
    private void Start()
    {
        _originPosition = transform.position;
    }
    
    private void Update()
    {
        switch (State)
        {
            case EMonsterState.Idle:
                Idle();
                break;
            case EMonsterState.Trace:
                Trace();
                break;
            case EMonsterState.Comeback:
                Comeback();
                break;
            case EMonsterState.Attack:
                Attack();
                break;
        }
    }
    
    // 1. 함수는 한 가지 일만 잘해야 한다
    // 2. 상태별 행동을 함수로 만든다
    private void Idle()
    {
        // 대기하는 상태
        // Todo. Idle 애니메이션 실행
        if (Vector3.Distance(transform.position, _player.transform.position) <= _detectDistance)
        {
            State = EMonsterState.Trace;
            Debug.Log("상태 전환: Idle->Trace");
        }
    }

    private void Trace()
    {
        // 플레이어를 쫓아가는 상태
        // Todo. Trace 애니메이션 재생
        float distance = Vector3.Distance(transform.position, _player.transform.position);

        if (distance > _detectDistance)
        {
            State = EMonsterState.Comeback;
            Debug.Log("상태 전환: Trace->Comeback");
        }
        
        Vector3 direction = (_player.transform.position - transform.position).normalized;
        _controller.Move(direction * _stats.MoveSpeed.Value * Time.deltaTime);

        if (distance <= _attackDistance)
        {
            State = EMonsterState.Attack;
            Debug.Log("상태 전환: Trace->Attack");
        }
    }

    private void Comeback()
    {
        // 제자리로 복귀한다 
        Vector3 direction = (_originPosition - transform.position).normalized;
        _controller.Move(direction * _stats.MoveSpeed.Value * Time.deltaTime);
        
        float distance = Vector3.Distance(transform.position, _originPosition);
        if (distance <= 0.1)
        {
            State = EMonsterState.Idle;
            Debug.Log("상태 전환: Comeback->Idle");
        }
    }

    private void Attack()
    {
        // 플레이어를 공격하는 상태
        float distance = Vector3.Distance(transform.position, _player.transform.position);
        if (distance > _attackDistance)
        {
            State = EMonsterState.Trace;
            Debug.Log("상태 전환: Attack->Trace");
            return;
        }

        _attackTimer += Time.deltaTime;
        if (_attackTimer >= _stats.AttackSpeed.Value)
        {
            _attackTimer = 0f;
            Debug.Log("플레이어 공격");
            
            PlayerStats playerStats = _player.GetComponent<PlayerStats>();
            playerStats.Health.TryConsume(_stats.Damage.Value);
        }
    }

    
    public bool TryTakeDamage(float damage)
    {
        if (State == EMonsterState.Hit || State == EMonsterState.Death)
        {
            return false;
        }
        
        _stats.Health.TryConsume(damage);

        if (_stats.Health.Value > 0)
        {
            // 히트 상태
            State = EMonsterState.Hit;
            StartCoroutine(Hit_Coroutine());
        }
        else
        {
            // 데스 상태
            State = EMonsterState.Death;
            StartCoroutine(Death_Coroutine());
        }

        return true;
    }

    private IEnumerator Hit_Coroutine()
    {
        // Todo. Hit 애니메이션 실행
        Vector3 hitDirection = (transform.position - _player.transform.position).normalized;
        hitDirection.y = 0f;
        
        _knockbackVelocity = hitDirection * _knockbackForce;
        
        float timer = 0f;
        float hitDuration = 0.25f;

        while (timer < hitDuration)
        {
            timer += Time.deltaTime;
            
            _controller.Move(_knockbackVelocity * Time.deltaTime);

            _knockbackVelocity = Vector3.Lerp(_knockbackVelocity, Vector3.zero, _knockbackDrag * Time.deltaTime);

            yield return null;
        }
        
        Debug.Log("Hit");
        
        State = EMonsterState.Idle;
    }

    private IEnumerator Death_Coroutine()
    {
        // Todo. Death 애니메이션 실행
        
        Debug.Log("Monster Die");
        
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
