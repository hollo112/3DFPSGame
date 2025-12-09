using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// 키보드를 누르면 캐릭터를 그 방향으로 이동 시키고 싶다
[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    // 필요 속성
    // 이동속도
    public float NormalSpeed = 5f;
    private float _currentMoveSpeed = 0f;
    // 중력
    public float Gravity = -9.81f;
    // 점프력
    public float JumpForce = 5f;
    private bool _canDoubleJump = true;
    private float _doubleJumpStaminaCost = 10f;
    
    // 스태미나
    public float MaxStamina = 100f;
    private float _currentStamina;
    private float _staminaUsePerSecond = 10f;
    private float _staminaRegainPerSecond = 5f;
    public float BoostSpeed = 8f;
    private Coroutine _rechargeRoutine;
    [SerializeField] private Slider _staminaSlider;
    
    private CharacterController _characterController;
    private float _yVelocity = 0f;  // 중력에 의해 누적될 y값 변수
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        _currentStamina = MaxStamina;
    }
    private void Update()
    {
        HandleStaminaBoost();
        
        // 중력을 누적한다
        _yVelocity += Gravity * Time.deltaTime;
        
        // 키보드 입력 받기
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        
        Vector3 direction = new Vector3(h, 0f, v).normalized;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_characterController.isGrounded)
            {
                _yVelocity = JumpForce;
                _canDoubleJump = true;
            }
            else if (_canDoubleJump)
            {
                if (_currentStamina <= _doubleJumpStaminaCost) return;
                
                _yVelocity = JumpForce;
                _canDoubleJump  = false;
                _currentStamina -= _doubleJumpStaminaCost;
                if (_rechargeRoutine == null)
                    _rechargeRoutine = StartCoroutine(RechargeStamina());
            }
        }
        Debug.Log("Current move speed: " + _currentMoveSpeed);
        
        direction = Camera.main.transform.TransformDirection(direction); 
        direction.y = _yVelocity;
        
        _characterController.Move(direction * _currentMoveSpeed * Time.deltaTime);
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
        _currentStamina = Mathf.Clamp(_currentStamina, 0f, MaxStamina);
        
        if (_staminaSlider != null)
        {
            _staminaSlider.value = _currentStamina;
        }
    }
    
    private void StartBoost()
    {
        _currentMoveSpeed = BoostSpeed;
        _currentStamina -= _staminaUsePerSecond * Time.deltaTime;
        
        if (_rechargeRoutine != null)
        {
            StopCoroutine(_rechargeRoutine);
            _rechargeRoutine = null;
        }
        
        if (_currentStamina <= 0f)
        {
            _currentStamina = 0f;
            StopBoost(); 
        }
    }

    private void StopBoost()
    {
        if (_currentMoveSpeed == NormalSpeed)
            return;

        _currentMoveSpeed = NormalSpeed;

        if (_rechargeRoutine == null)
            _rechargeRoutine = StartCoroutine(RechargeStamina());
    }
    
    private IEnumerator RechargeStamina()
    {
        yield return new WaitForSeconds(1f);

        while (_currentStamina < MaxStamina)
        {
            _currentStamina += _staminaRegainPerSecond / 10;
            if (_currentStamina > MaxStamina)
                _currentStamina = MaxStamina;

            yield return new WaitForSeconds(0.1f);
        }

        _rechargeRoutine = null;
    }
}
