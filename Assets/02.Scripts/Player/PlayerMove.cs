using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// 키보드를 누르면 캐릭터를 그 방향으로 이동 시키고 싶다
[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    [Header("Speed")]
    public float NormalSpeed = 5f;
    public float BoostSpeed = 8f;
    private float _currentMoveSpeed = 0f;
    
    [Header("Gravity")]
    public float Gravity = -9.81f;
    [Header("Jump")]
    public float JumpForce = 5f;
    private bool _canDoubleJump = true;
    private float _yVelocity = 0f;  // 중력에 의해 누적될 y값 변수
    
    [Header("Stamina")]
    private const float MaxStamina = 100f;
    private float _currentStamina;
    
    private float _staminaUsePerSecond = 10f;
    private float _staminaRegainPerSecond = 5f;
    private const float DoubleJumpStaminaCost = 10f;
    private const float StaminaRechargeDelay = 1f; 
    
    private Coroutine _rechargeRoutine;
    
    [Header("UI")]
    [SerializeField] private Slider _staminaSlider;
    
    private CharacterController _characterController;
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        _currentStamina = MaxStamina;
        _currentMoveSpeed = NormalSpeed;
    }
    private void Update()
    {
        HandleStaminaBoost();
        HandleJumpInput();
        ApplyMovement();
        UpdateUI();
    }
    
    private void HandleStaminaBoost()
    {
        bool isBoosting = Input.GetKey(KeyCode.LeftShift) && _currentStamina > 0f;
        
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
        _currentMoveSpeed = BoostSpeed;
        ConsumeStamina(_staminaUsePerSecond * Time.deltaTime);

        StopRechargeRoutine();
    }

    private void StopBoost()
    {
        if (_currentMoveSpeed == NormalSpeed) return;

        _currentMoveSpeed = NormalSpeed;

        StartRechargeRoutine();
    }
    
    private void HandleJumpInput()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;

        if (_characterController.isGrounded)
        {
            _yVelocity = JumpForce;
            _canDoubleJump = true;
        }
        else if (_canDoubleJump && TryUseStamina(DoubleJumpStaminaCost))
        {
            _yVelocity = JumpForce;
            _canDoubleJump = false;
        }
    }
    
    private void ApplyMovement()
    {
        _yVelocity += Gravity * Time.deltaTime;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(h, 0, v).normalized;

        direction = Camera.main.transform.TransformDirection(direction);
        direction.y = _yVelocity;

        _characterController.Move(direction * _currentMoveSpeed * Time.deltaTime);
    }
    
    private bool TryUseStamina(float amount)
    {
        if (_currentStamina < amount) return false;

        ConsumeStamina(amount);
        StopRechargeRoutine();     
        StartRechargeRoutine();   

        return true;
    }

    private void ConsumeStamina(float amount)
    {
        _currentStamina -= amount;
        _currentStamina = Mathf.Clamp(_currentStamina, 0f, MaxStamina);
    }

    private void StartRechargeRoutine()
    {
        if (_rechargeRoutine == null)
            _rechargeRoutine = StartCoroutine(RechargeStamina());
    }

    private void StopRechargeRoutine()
    {
        if (_rechargeRoutine != null)
        {
            StopCoroutine(_rechargeRoutine);
            _rechargeRoutine = null;
        }
    }

    private const float StaminaRechargeInterval = 0.1f;
    private const float StaminaRechargeStepsPerSecond = 10f;
    private IEnumerator RechargeStamina()
    {
        yield return new WaitForSeconds(StaminaRechargeDelay); 

        while (_currentStamina < MaxStamina)
        {
            _currentStamina += _staminaRegainPerSecond / StaminaRechargeStepsPerSecond;

            if (_currentStamina > MaxStamina)
                _currentStamina = MaxStamina;

            yield return new WaitForSeconds(StaminaRechargeInterval);
        }

        _rechargeRoutine = null;
    }
    
    private void UpdateUI()
    {
        if (_staminaSlider != null)
            _staminaSlider.value = _currentStamina;
    }
}
