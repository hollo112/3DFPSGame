using System;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour, IDamageable, IMonsterContext
{
    public IMonsterState State { get; private set; }
    
    public Transform Transform => transform;
    public Animator Animator => _animator;
    public NavMeshAgent NavMeshAgent => _navMeshAgent;
    public GameObject Player => _player;
    public MonsterStats Stats => _stats;
    public float AttackDistance => _attackDistance;
    public float DetectDistance => _detectDistance;
    
    private GameObject _player;
    private const string PlayerTag = "Player";

    private PlayerHealth _playerHealth;
    public PlayerHealth PlayerHealth => _playerHealth;

    private CharacterController _controller;
    public CharacterController Controller => _controller;

    private MonsterStats _stats;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    
    [SerializeField] GameObject _bloodEffectPrefab;
    [SerializeField] GameObject _coinPrefab;
    [SerializeField] private float _detectDistance = 15f;
    [SerializeField] private float _attackDistance = 2f;
    [SerializeField] private float _knockbackDrag = 7f;
    [SerializeField] private float _patrolRadius = 6f;
    [SerializeField] private float _pointReach = 2f;
    [SerializeField] private float _patrolInterval = 1.5f;
    [SerializeField] private float _deathDelay = 2f;
    [SerializeField] private int _coinCount = 10;
    public float KnockbackDrag => _knockbackDrag;
    public float PatrolRadius => _patrolRadius;
    public float PointReach => _pointReach;
    public float PatrolInterval => _patrolInterval;
    public float DeathDelay => _deathDelay;
    
    private Vector3 _originPosition;
    public Vector3 OriginPosition => _originPosition;

    private float _yVelocity = 0f;
    [SerializeField] private float _gravity = -9.81f;
    public float YVelocity => _yVelocity;

    private float _rotateSpeed = 10f;

    public event Action<Damage> OnDamaged;
    
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _stats = GetComponent<MonsterStats>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag(PlayerTag);
        if (_player != null)
        {
            _playerHealth = _player.GetComponent<PlayerHealth>();
        }
        _animator =  GetComponentInChildren<Animator>();
    }
    
    private void Start()
    {
        _originPosition = transform.position;
        ChangeState(new IdleState(this));
        _navMeshAgent.speed = _stats.WalkSpeed.Value;
        _navMeshAgent.stoppingDistance = AttackDistance;
        
    }
    
    public void ChangeState(IMonsterState newState)
    {
        State?.Exit();
        State = newState;
        State.Enter();
    }
    
    private void Update()
    {
        if (GameManager.Instance.State != EGameState.Playing) return;
        State?.Update();
    }
    
    public bool TryTakeDamage(Damage damage)
    {
        if (State is DeathState)
        {
            return false;
        }
        
        _stats.Health.ConsumeClamped(damage.Value);
        
        // GameObject bloodEffect = Instantiate(_bloodEffectPrefab, transform.position, Quaternion.identity, transform);
        // bloodEffect.transform.forward = damage.Normal;

        if (_stats.Health.Value <= 0)
        {
            ChangeState(new DeathState(this));
            OnDamaged?.Invoke(damage);
            return true;
        }

        if (State is HitState)
        {
            OnDamaged?.Invoke(damage);
            return true;
        }

        ChangeState(new HitState(this, State, damage));
        OnDamaged?.Invoke(damage);
        return true;
    }
    
    public void MoveTo(Vector3 position)
    {
        _navMeshAgent.SetDestination(position);
    }
    
    public void MoveRaw(Vector3 velocity)
    {
        Vector3 move = velocity;
        move.y = _yVelocity;
        _controller.Move(move * Time.deltaTime);
        ApplyGravity();
    }
    
    public void RotateToward(Vector3 direction)
    {
        Vector3 rotateDirection = direction;
        rotateDirection.y = 0f; 
    
        if (rotateDirection.sqrMagnitude < 0.0001f)
            return;

        Quaternion targetRot = Quaternion.LookRotation(rotateDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, _rotateSpeed * Time.deltaTime);
    }
    
    private void ApplyGravity()
    {
        if (_controller.isGrounded && _yVelocity < 0)
        {
            _yVelocity = -1f;
        }
        else
        {
            _yVelocity += _gravity * Time.deltaTime;
        }
    }

    public void MakeCoin()
    {
        for (int i = 0; i < _coinCount; i++)
        {
            Debug.Log("Making coin");
            Instantiate(_coinPrefab, transform.position, Quaternion.identity);    
        }
    }
}
