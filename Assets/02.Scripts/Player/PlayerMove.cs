using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

// 키보드를 누르면 캐릭터를 그 방향으로 이동 시키고 싶다
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMove : MonoBehaviour
{
    [Serializable]
    public class MoveConfig
    {
        public float Gravity;
        public float RunStaminaCost;
        public float JumpStaminaCost;
    }
    
    public MoveConfig Config;
    
    private float _yVelocity = 0f;
    private float _speed;
    private bool _canDoubleJump;
    private const float GroundStickVelocity = -2f;
    private Coroutine _rechargeRoutine;
    
    private CharacterController _characterController;
    private PlayerStats _stats;
    private NavMeshAgent _agent;
    private RaycastHit _rayHitPoint;
    
    public Vector3 MoveVector;
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _stats =  GetComponent<PlayerStats>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (GameManager.Instance.State != EGameState.Playing) return;
        
        HandleStaminaBoost();
        HandleJumpInput();
        
        if (GameManager.Instance.ViewMode == ECameraViewMode.Top)
        {
            ApplyMovementByMouse();
            return;
        }
        
        ApplyMovementByKeyboard();
    }
    
    private void HandleStaminaBoost()
    {
        bool isBoosting = Input.GetKey(KeyCode.LeftShift) && _stats.Stamina.Value > 0f;
        
        if (isBoosting)
        {
            StartBoost();
        }
        else
        {
            StopBoost();
        }
    }
    
    private void StartBoost()
    {
        if (_stats.Stamina.Value < 0.1f)
        {
            StopBoost();
            return;
        }
        _speed = _stats.RunSpeed.Value;
        _stats.Stamina.TryConsume(Config.RunStaminaCost * Time.deltaTime);
    }

    private void StopBoost()
    {
        _speed = _stats.WalkSpeed.Value;
    }
    
    private void HandleJumpInput()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;

        if (_characterController.isGrounded)
        {
            _yVelocity = _stats.JumpPower.Value;
            _canDoubleJump = true;
        }
        else if (_canDoubleJump && _stats.Stamina.Value > Config.JumpStaminaCost)
        {
            _yVelocity = _stats.JumpPower.Value;
            _stats.Stamina.TryConsume(Config.JumpStaminaCost);
            _canDoubleJump = false;
        }
    }
    
    private void ApplyMovementByKeyboard()
    {
        if (_characterController.isGrounded && _yVelocity < 0f)
        {
            _yVelocity = GroundStickVelocity; 
        }
        else
        {
            _yVelocity += Config.Gravity * Time.deltaTime;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(h, 0, v).normalized;

        direction = Camera.main.transform.TransformDirection(direction);
        direction.y = _yVelocity;

        MoveVector = direction * _speed * Time.deltaTime;
        _characterController.Move(direction * _speed * Time.deltaTime);
    }

    private void ApplyMovementByMouse()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out _rayHitPoint))
            {
                _agent.destination = _rayHitPoint.point;
                _agent.speed = _speed;
            }
        }
    }
}
