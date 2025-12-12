using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour, IDamageable
{
    public EMonsterState State = EMonsterState.Idle;
    [SerializeField] private GameObject _player;
    private PlayerHealth _playerHealth;
    private CharacterController _controller;
    private MonsterStats _stats;

    [SerializeField] private float _detectDistance = 8f;
    [SerializeField] private float _attackDistance = 2f;
    [SerializeField] private float _knockbackForce = 4f;
    [SerializeField] private float _knockbackDrag = 7f;
    [SerializeField] private float _patrolRadius = 3f;
    [SerializeField] private float _patrolPointReach = 0.3f;
    [SerializeField] private float _patrolInterval = 3f;
    
    private float _attackTimer = 0;
    private float _patrolTimer = 0;
    private Vector3 _originPosition;
    private Vector3 _knockbackVelocity;
    private Vector3 _currentPatrolTarget;
    
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _stats = GetComponent<MonsterStats>();
        _playerHealth = _player.GetComponent<PlayerHealth>();
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
            case EMonsterState.Patrol:
                Patrol();
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
        
        _patrolTimer += Time.deltaTime;
        if (_patrolTimer >= _patrolInterval)
        {
            _patrolTimer = 0f;
            State = EMonsterState.Patrol;
            Debug.Log("상태 전환: Idle->Patrol");
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
            State = EMonsterState.Patrol;
            Debug.Log("상태 전환: Comeback->Patrol");
        }
        
        // 복귀중 플레이어가 근처에 있으면 다시 추격
        float playerDistance = Vector3.Distance(transform.position, _player.transform.position);
        if (playerDistance < _detectDistance)
        {
            State = EMonsterState.Trace;
            Debug.Log("상태 전환: Comeback->Trace");
        }
    }

    private void Patrol()
    {
        if (_currentPatrolTarget == Vector3.zero)
        {
            _currentPatrolTarget = _originPosition + 
                                   new Vector3(Random.Range(-_patrolRadius, _patrolRadius), 0, Random.Range(-_patrolRadius, _patrolRadius));
        }

        if (Vector3.Distance(transform.position, _currentPatrolTarget) < _patrolPointReach)
        {
            _currentPatrolTarget = Vector3.zero;
            State = EMonsterState.Idle;
            Debug.Log("상태 전환: Patrol->Idle");
        }

        Vector3 direction = (_currentPatrolTarget - transform.position).normalized;
        _controller.Move(direction * _stats.MoveSpeed.Value * Time.deltaTime);

        // 순찰 중 플레이어 발견
        if (Vector3.Distance(transform.position, _player.transform.position) <= _detectDistance)
        {
            State = EMonsterState.Trace;
            Debug.Log("Patrol->Trace");
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
            Damage damage = new Damage(_stats.Damage.Value, transform.position);
            
            _playerHealth.TryTakeDamage(damage);
        }
    }

    
    public bool TryTakeDamage(Damage damage)
    {
        if (State == EMonsterState.Hit || State == EMonsterState.Death)
        {
            return false;
        }
        
        _stats.Health.ConsumeClamped(damage.Value);

        if (_stats.Health.Value > 0)
        {
            // 히트 상태
            EMonsterState previousState = State;
            State = EMonsterState.Hit;
            StartCoroutine(Hit_Coroutine(previousState, damage.AttackerPosition));
        }
        else
        {
            // 데스 상태
            State = EMonsterState.Death;
            StartCoroutine(Death_Coroutine());
        }

        return true;
    }

    private IEnumerator Hit_Coroutine(EMonsterState previousState, Vector3 attackerPosition)
    {
        // Todo. Hit 애니메이션 실행
        Vector3 hitDirection = (transform.position - attackerPosition).normalized;
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
        
        State = previousState;
    }

    private IEnumerator Death_Coroutine()
    {
        // Todo. Death 애니메이션 실행
        
        Debug.Log("Monster Die");
        
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
