using System;
using System.Collections;
using UnityEngine;

public class PlayerGunFire : MonoBehaviour
{
    [SerializeField] private RayGun rayGun;
    [SerializeField] private CameraRecoil recoil;
    private float _reloadTime = 0.6f;
    private bool _isReloading = false;
    public float ReloadProgress { get; private set; }
    public event Action<float> OnReloadProgressChanged;
    public event Action OnReloadStarted;
    public event Action OnReloadFinished;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!_isReloading)
            {
                StartCoroutine(Reload_Coroutine());
            }
        }
            

        if (Input.GetMouseButton(0))
        {
            if (_isReloading) return;
            
            if (rayGun.TryFire())
            {
                recoil.PlayRecoil(rayGun.RecoilPower);
            }
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
        rayGun.Reload();
        
        _isReloading = false;
        OnReloadFinished?.Invoke();
    }
}