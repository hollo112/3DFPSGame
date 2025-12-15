using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_HitEffect : MonoBehaviour
{
    [SerializeField] private Image _hitEffectImage;
    [SerializeField] private float _fadeInTime = 0.1f;
    [SerializeField] private float _fadeOutTime = 0.3f;
    [SerializeField] private PlayerHealth _playerHealth;
    
    private Tween _hitTween;
    
    private void Start()
    {
        SetAlpha(0f);
    }
    
    private void OnEnable()
    {
        _playerHealth.OnDamaged += HandleDamaged;
    }

    private void OnDisable()
    {
        _playerHealth.OnDamaged -= HandleDamaged;
    }
    
    private void SetAlpha(float a)
    {
        Color color = _hitEffectImage.color;
        color.a = a;
        _hitEffectImage.color = color;
    }
    
    private void HandleDamaged(Damage damage)
    {
        Hit();
    }
    
    public void Hit()
    {
        _hitTween?.Kill();

        _hitTween = DOTween.Sequence()
            .Append(_hitEffectImage.DOFade(1f, _fadeInTime))
            .Append(_hitEffectImage.DOFade(0f, _fadeOutTime))
            .SetEase(Ease.OutQuad);
    }
}
