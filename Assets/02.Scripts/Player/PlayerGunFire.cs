using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerGunFire : MonoBehaviour, IPlayerFire
{
    [SerializeField] private RayGun _rayGun;
    [SerializeField] private CameraRecoil _cameraRecoil;
    
    private EZoomMode _zoomMode = EZoomMode.Normal;
    [SerializeField] private GameObject _normalCrosshair;
    [SerializeField] private GameObject _zoomInCrosshair;
    
    public bool IsAutomatic => true;
    private PlayerStats _playerStats;
    private Animator _animator;

    
    private float _reloadTime = 0.6f;
    private bool _isReloading = false;
    public float ReloadProgress { get; private set; }
    public event Action<float> OnReloadProgressChanged;
    public event Action OnReloadStarted;
    public event Action OnReloadFinished;

    private void Awake()
    {
        _playerStats = GetComponent<PlayerStats>();
        _animator = GetComponentInChildren<Animator>();
    }
    public void OnSelect()
    {
        
    }
    public void OnDeselect()
    {
        _isReloading = false;
        StopAllCoroutines();
    }

    public void Fire()
    {
        if (_isReloading) return;

        if (_rayGun.TryFire(_playerStats.Damage.Value))
        {
            _animator.SetTrigger("Fire");
            _cameraRecoil.PlayRecoil(_rayGun.RecoilData);
        }
    }
    
    public void Reload()
    {
        if (!_isReloading)
            StartCoroutine(Reload_Coroutine());
    }
    
    private void Update()
    {
        ZoomModeCheck();
    }

    private void ZoomModeCheck()
    {
        if (Input.GetMouseButton(1))
        {
            _zoomMode = EZoomMode.ZoomIn;
            _normalCrosshair.SetActive(false);
            _zoomInCrosshair.SetActive(true);
            Camera.main.fieldOfView = 10f;
        }
        else
        {
            _zoomMode = EZoomMode.Normal;
            _normalCrosshair.SetActive(true);
            _zoomInCrosshair.SetActive(false);
            Camera.main.fieldOfView = 60f;
        }
    }

    private IEnumerator Reload_Coroutine()
    {
        _isReloading = true;
        ReloadProgress = 0f;
        
        OnReloadStarted?.Invoke();
        
        float timer = 0f;

        while (timer < _reloadTime)
        {
            timer += Time.deltaTime;
            ReloadProgress = timer / _reloadTime;
            
            OnReloadProgressChanged?.Invoke(ReloadProgress);
            
            yield return null;
        }
        _rayGun.Reload();
        
        _isReloading = false;
        OnReloadFinished?.Invoke();
    }
}