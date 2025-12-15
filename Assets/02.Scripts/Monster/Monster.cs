using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour, IDamageable
{
    public IMonsterState State { get; private set; }
    
    private GameObject _player;
    private const string PlayerTag = "Player";
    public GameObject Player => _player;
    private PlayerHealth _playerHealth;
    public PlayerHealth PlayerHealth => _playerHealth;
    private CharacterController _controller;
    public CharacterController Controller => _controller;
    private MonsterStats _stats;
    public  MonsterStats Stats => _stats;

    public float DetectDistance {get; private set;} = 10f;
    public float AttackDistance {get; private set;} = 2f;
    public float KnockbackDrag {get; private set;} = 7f;
    public float PatrolRadius{ get; private set; } = 3f;
    public float PointReach{ get; private set; } = 0.1f;
    public float PatrolInterval{ get; private set; } = 2f;
    public float HitDuration{ get; private set; } = 0.25f;
    public float DeathDelay{ get; private set; } = 2f;
    private Vector3 _originPosition;
    public Vector3 OriginPosition => _originPosition;
    
    private float _yVelocity = 0f;
    [SerializeField] private float _gravity = -9.81f;
    public float YVelocity => _yVelocity;
    private float _rotateSpeed = 10f;
    
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _stats = GetComponent<MonsterStats>();
        _player = GameObject.FindGameObjectWithTag(PlayerTag);
        if (_player != null)
        {
            _playerHealth = _player.GetComponent<PlayerHealth>();
        }
    }
    private void Start()
    {
        _originPosition = transform.position;
        ChangeState(new IdleState(this));
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
        if (State is HitState || State is DeathState)
        {
            return false;
        }
        
        _stats.Health.ConsumeClamped(damage.Value);

        if (_stats.Health.Value > 0)
        {
            // 히트 상태
            ChangeState(new HitState(this, State, damage));
        }
        else
        {
            // 데스 상태
            ChangeState(new DeathState(this));
        }

        return true;
    }
    
    public void Move(Vector3 direction)
    {
        RotateToward(direction); 
        
        Vector3 move = direction;
        move.y = _yVelocity;
        _controller.Move(move * _stats.MoveSpeed.Value * Time.deltaTime);
        ApplyGravity();
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
}
