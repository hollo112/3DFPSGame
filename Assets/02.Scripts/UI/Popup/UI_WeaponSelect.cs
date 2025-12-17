using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_WeaponSelect : MonoBehaviour
{
    [Header("WeaponController")] 
    [SerializeField] private WeaponController _weaponController;
    [Header("이미지")]
    [SerializeField] private Image _gunImage;
    [SerializeField] private Image _bombImage;

    [Header("알파값")]
    [SerializeField] private float _inactiveAlpha = 0.3f;

    [Header("애니메이션")]
    [SerializeField] private float _punchScale = 0.15f;
    [SerializeField] private float _punchDuration = 0.25f;
    
    [Header("패널 표시 시간")]
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _showDuration = 1.2f;
    [SerializeField] private float _fadeOutDuration = 0.25f;
    
    private Image _currentActive;
    
    private Tween _panelTween;
    private void Start()
    {
        _weaponController.OnWeaponChanged += OnWeaponChanged;
        
        _canvasGroup.alpha = 0f;
        _currentActive = _gunImage;
    }

    private void OnDestroy()
    {
        _weaponController.OnWeaponChanged -= OnWeaponChanged;
    }

    private void OnWeaponChanged(IPlayerFire weapon)
    {
        ShowPanel();
        
        Image target = GetImageByWeapon(weapon);
        if (target == _currentActive) return;

        if (_currentActive != null)
        {
            SetAlpha(_currentActive, _inactiveAlpha);
            _currentActive.transform.localScale = Vector3.one;
        }

        _currentActive = target;
        SetAlpha(_currentActive, 1f);

        _currentActive.transform.DOPunchScale(Vector3.one * _punchScale, _punchDuration);
    }

    private Image GetImageByWeapon(IPlayerFire weapon)
    {
        if (weapon is PlayerGunFire) return _gunImage;
        if (weapon is PlayerBombFire) return _bombImage;

        return _gunImage;
    }

    private void SetAlpha(Image image, float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }
    
    private void ShowPanel()
    {
        _panelTween?.Kill();

        _canvasGroup.alpha = 1f;

        _panelTween = DOTween.Sequence()
            .AppendInterval(_showDuration)
            .Append(
                _canvasGroup
                    .DOFade(0f, _fadeOutDuration)
                    .SetEase(Ease.InQuad)
            );
    }
}
