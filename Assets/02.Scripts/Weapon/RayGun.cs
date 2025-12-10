using System.Collections;
using UnityEngine;

public class RayGun : MonoBehaviour, IFireMode
{
    [SerializeField] private Transform _fireTransform;
    [SerializeField] private ParticleSystem _hitEffect;
    private Magazine _magazine;
    
    public float FireCooldown = 0.1f;
    private bool _isCoolDown = false;

    public float RecoilPower { get; private set; } = 3f;
    
    private void Awake()
    {
        _magazine = GetComponent<Magazine>();
    }

    public bool TryFire()
    {
        if (_isCoolDown) return false;      
        if (_magazine.MagzineSize <= 0) return false;

        Fire();
        StartCoroutine(FireCooldownRoutine());
        
        return true;
    }

    private IEnumerator FireCooldownRoutine()
    {
        _isCoolDown = true;
        yield return new WaitForSeconds(FireCooldown);
        _isCoolDown = false;
    }

    private void Fire()
    {
        _magazine.DecreaseMagazineSize(1);

        Ray ray = new Ray(_fireTransform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            _hitEffect.transform.position = hitInfo.point;
            _hitEffect.transform.forward = hitInfo.normal;
            _hitEffect.Play();
        }
    }

    public void Reload()
    {
        _magazine.Reload();
    }
}
