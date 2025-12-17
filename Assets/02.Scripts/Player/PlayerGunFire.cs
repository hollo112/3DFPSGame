using System;
using System.Collections;
using UnityEngine;

public class PlayerGunFire : MonoBehaviour, IPlayerFire
{
    [SerializeField] private RayGun _rayGun;
    [SerializeField] private CameraRecoil _cameraRecoil;
    public bool IsAutomatic => true;
    private PlayerStats _playerStats;
    
    private float _reloadTime = 0.6f;
    private bool _isReloading = false;
    public float ReloadProgress { get; private set; }
    public event Action<float> OnReloadProgressChanged;
    public event Action OnReloadStarted;
    public event Action OnReloadFinished;

    private void Awake()
    {
        _playerStats = GetComponent<PlayerStats>();
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
            _cameraRecoil.PlayRecoil(_rayGun.RecoilData);
        }
    }
    
    public void Reload()
    {
        if (!_isReloading)
            StartCoroutine(Reload_Coroutine());
    }
    
    // private void Update()
    // {
    //     if (GameManager.Instance.State != EGameState.Playing) return;
    //     if (!CursorManager.Instance.IsLocked) return;
    //     if (Input.GetKeyDown(KeyCode.R))
    //     {
    //         if (!_isReloading)
    //         {
    //             StartCoroutine(Reload_Coroutine());
    //         }
    //     }
    //         
    //
    //     if (Input.GetMouseButton(0))
    //     {
    //         if (_isReloading) return;
    //         
    //         if (_rayGun.TryFire(_playerStats.Damage.Value))
    //         {
    //             _cameraRecoil.PlayRecoil(_rayGun.RecoilData);
    //         }
    //     }
    // }

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