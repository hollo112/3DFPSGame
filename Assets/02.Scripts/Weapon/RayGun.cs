using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class RayGun : MonoBehaviour, IFireMode
{
    [SerializeField] private Transform _fireTransform;
    [SerializeField] private ParticleSystem _hitEffect;
    
    [SerializeField] private GameObject[] _muzzleFlash;
    private float _muzzleDuration = 0.05f;
    [SerializeField] private Transform _muzzleTransform;
    [SerializeField] private Transform _trailTransform;
    private const float MaxFireDistance = 3000f;
    [SerializeField]private float _trailSpeed = 100f;
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
        Vector3 hitPoint = _fireTransform.position + fireDirection * MaxFireDistance;
        
        if (Physics.Raycast(fireRay, out RaycastHit fireHit, MaxFireDistance))
        {
            hitPoint = fireHit.point;
            _hitEffect.transform.position = fireHit.point;
            _hitEffect.transform.forward = fireHit.normal;
            _hitEffect.Play();

            if (fireHit.collider.TryGetComponent(out IDamageable monster))
            {
                Damage damage = new Damage()
                {
                    Value = damageValue,
                    AttackerPosition = transform.position,
                    Normal = fireHit.normal,
                };
                monster.TryTakeDamage(damage);
            }
        }
        
        BulletTrail trail = BulletTrailPoolManager.Instance.Get();
        trail.Play(_trailTransform.position, hitPoint, _trailSpeed);
        
        StartCoroutine(MuzzleFlash_Coroutine());
    }

    private IEnumerator MuzzleFlash_Coroutine()
    {
        GameObject muzzleEffect = _muzzleFlash[Random.Range(0, _muzzleFlash.Length)];
        
        muzzleEffect.transform.position = _muzzleTransform.position;
        muzzleEffect.transform.rotation = _muzzleTransform.rotation;
        muzzleEffect.SetActive(true);
        
        yield return new WaitForSeconds(_muzzleDuration);
        
        muzzleEffect.SetActive(false);
    }

    public void Reload()
    {
        _magazine.Reload();
    }
}
