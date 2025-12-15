using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Monster))]
public class MonsterHealthBar : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image _guageImage;
    [SerializeField] private Image _delayedGauge;
    [SerializeField] private Transform _healthbarTransform;
    [Header("Color")]
    [SerializeField] private Color _healthbarColor;
    [SerializeField] private Color _hitFlashColor = Color.white;
    [Header("Settings")]
    [SerializeField] private float _delayBeforeReduce = 0.2f;
    [SerializeField] private float _delayedReduceTime = 0.4f;
    [SerializeField] private float _hitFlashStartDuration = 0.05f;
    [SerializeField] private float _hitFlashEndDuration = 0.3f;
    [SerializeField] private float _shakeDuration = 0.15f;
    [SerializeField] private float _shakeStrength = 0.03f;
    [SerializeField] private int _shakeVibrato = 30;
    [SerializeField] private float _shakeRandomness = 10f;
    
    private Monster _monster;
    private MonsterStats _monsterStats;
    private Tween _delayedTween;
    private Tween _flashTween;
    private Tween _shakeTween;
    private void Awake()
    {
        _monsterStats = GetComponent<MonsterStats>();
        _monster = GetComponent<Monster>();
    }

    private void Start()
    {
        _guageImage.color = _healthbarColor;    
    }
    
    private void OnEnable()
    {
        _monster.OnDamaged += HandleDamaged;
    }

    private void OnDisable()
    {
        _monster.OnDamaged -= HandleDamaged;
    }
    
    private void LateUpdate()
    {
        // 빌보드 기법 : 카메라의 위치와 회전에 상관없이 항상 정면을 바라보게 하는 기법
        _healthbarTransform.forward = Camera.main.transform.forward;
    }

    private void HandleDamaged(Damage damage)
    {
        ChangeGauge();
        ChangeDelayGauge();
        FlashGauge();
        ShakeGauge();
    }

    private void ChangeGauge()
    {
        _guageImage.fillAmount = GetHealthNormalized();
    }

    private void ChangeDelayGauge()
    {
        float targetFill = GetHealthNormalized();
        
        _delayedTween?.Kill();
        _delayedTween = _delayedGauge
            .DOFillAmount(targetFill, _delayedReduceTime)
            .SetDelay(_delayBeforeReduce)
            .SetEase(Ease.OutQuad);
    }

    private void FlashGauge()
    {
        _flashTween?.Kill();
        _flashTween = DOTween.Sequence()
            .Append(_guageImage.DOColor(_hitFlashColor, _hitFlashStartDuration))
            .Append(_guageImage.DOColor(_healthbarColor, _hitFlashEndDuration));
    }

    private void ShakeGauge()
    {
        _shakeTween?.Kill();
        _shakeTween = _healthbarTransform
            .DOShakePosition(
                _shakeDuration,
                _shakeStrength,
                _shakeVibrato,
                _shakeRandomness
            );
    }
    
    private float GetHealthNormalized()
    {
        return _monsterStats.Health.Value / _monsterStats.Health.MaxValue;
    }
}
