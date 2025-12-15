using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Monster))]
public class MonsterHealthBar : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image _guageimage;
    [SerializeField] private Image _delayedGague;
    [SerializeField] private Transform _healthbarTransform;
    [Header("Color")]
    [SerializeField] private Color _healthbarColor;
    [SerializeField] private Color _hitFlashColor = Color.white;
    [Header("Settings")]
    [SerializeField] private float _delayBeforeReduce = 0.2f;
    [SerializeField] private float _delayedReduceTime = 0.4f;
    [SerializeField] private float _hitFlashStartDuration = 0.05f;
    [SerializeField] private float _hitFlashEndDuration = 0.2f;
    
    private Monster _monster;
    private MonsterStats _monsterStats;
    private float _lastHealth = -1f;
    private Tween _delayedTween;
    private Tween _flashTween;
    
    private void Awake()
    {
        _monsterStats = GetComponent<MonsterStats>();
        _monster = GetComponent<Monster>();
    }

    private void Start()
    {
        _guageimage.color = _healthbarColor;    
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
        if (_lastHealth != _monsterStats.Health.Value)
        {
            _lastHealth = _monsterStats.Health.Value;
            _guageimage.fillAmount = GetHealthNormalized();
        }
        
        // 빌보드 기법 : 카메라의 위치와 회전에 상관없이 항상 정면을 바라보게 하는 기법
        _healthbarTransform.forward = Camera.main.transform.forward;
    }

    private void HandleDamaged(Damage damage)
    {
        float targetFill = GetHealthNormalized();

        _delayedTween?.Kill();
        _delayedTween = _delayedGague
            .DOFillAmount(targetFill, _delayedReduceTime)
            .SetDelay(_delayBeforeReduce)
            .SetEase(Ease.OutQuad);

        _flashTween?.Kill();
        _flashTween = DOTween.Sequence()
            .Append(_guageimage.DOColor(_hitFlashColor, _hitFlashStartDuration))
            .Append(_guageimage.DOColor(_healthbarColor, _hitFlashEndDuration));
    }
    
    private float GetHealthNormalized()
    {
        return _monsterStats.Health.Value / _monsterStats.Health.MaxValue;
    }
}
