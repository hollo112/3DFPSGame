using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour, IDamageable
{
    public IMonsterState State { get; private set; }
    
    [SerializeField] private GameObject _player;
    public GameObject Player => _player;
    private PlayerHealth _playerHealth;
    public PlayerHealth PlayerHealth => _playerHealth;
    private CharacterController _controller;
    public CharacterController Controller => _controller;
    private MonsterStats _stats;
    public  MonsterStats Stats => _stats;

    public float DetectDistance {get; private set;} = 8f;
    public float AttackDistance {get; private set;} = 2f;
    public float KnockbackForce {get; private set;} = 4f;
    public float KnockbackDrag {get; private set;} = 7f;
    public float PatrolRadius{ get; private set; } = 3f;
    public float PatrolPointReach{ get; private set; } = 0.3f;
    public float PatrolInterval{ get; private set; } = 3f;
    
    private Vector3 _originPosition;
    public Vector3 OriginPosition => _originPosition;
    
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _stats = GetComponent<MonsterStats>();
        _playerHealth = _player.GetComponent<PlayerHealth>();
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
}
