using System.Collections;
using UnityEngine;

public class RayGun : MonoBehaviour, IFireMode
{
    [SerializeField] private Transform _fireTransform;
    [SerializeField] private ParticleSystem _hitEffect;
    private const float MaxFireDistance = 1000f;
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
        
        Vector3 targetPoint;
        Ray centerRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(centerRay, out RaycastHit centerHit, MaxFireDistance))
        {
            targetPoint = centerHit.point;
        }
        else
        {
            targetPoint = Camera.main.transform.position + Camera.main.transform.forward * MaxFireDistance;
        }
        
        Vector3 fireDirection = (targetPoint - _fireTransform.position).normalized;
        Ray fireRay = new Ray(_fireTransform.position, fireDirection);

        if (Physics.Raycast(fireRay, out RaycastHit fireHit, MaxFireDistance))
        {
            _hitEffect.transform.position = fireHit.point;
            _hitEffect.transform.forward = fireHit.normal;
            _hitEffect.Play();

            if (fireHit.collider.TryGetComponent(out IDamageable monster))
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
