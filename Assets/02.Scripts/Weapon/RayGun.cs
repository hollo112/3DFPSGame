using System.Collections;
using UnityEngine;

public class RayGun : MonoBehaviour, IFireMode
{
    [SerializeField] private Transform _fireTransform;
    [SerializeField] private ParticleSystem _hitEffect;
    public RecoilData RecoilData{get; private set;}
    private Magazine _magazine;
    
    public float FireCooldown = 0.1f;
    private bool _isCoolDown = false;
    
    private void Awake()
    {
        _magazine = GetComponent<Magazine>();
        RecoilData = GetComponent<RecoilData>();
    }

    public bool TryFire(float damage)
    {
        if (_isCoolDown) return false;      
        if (_magazine.MagzineSize <= 0) return false;

        Fire(damage);
        StartCoroutine(FireCooldownRoutine());
        
        return true;
    }

    private IEnumerator FireCooldownRoutine()
    {
        _isCoolDown = true;
        yield return new WaitForSeconds(FireCooldown);
        _isCoolDown = false;
    }

    private void Fire(float damageValue)
    {
        _magazine.DecreaseMagazineSize(1);

        Ray ray = new Ray(_fireTransform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            _hitEffect.transform.position = hitInfo.point;
            _hitEffect.transform.forward = hitInfo.normal;
            _hitEffect.Play();

            if (hitInfo.collider.TryGetComponent(out IDamageable monster))
            {
                Damage damage = new Damage(damageValue, transform.position);
                monster.TryTakeDamage(damage);
            }
        }
    }

    public void Reload()
    {
        _magazine.Reload();
    }
}
