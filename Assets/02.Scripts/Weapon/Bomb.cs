using UnityEngine;
using UnityEngine.Pool;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float _explosionRadius = 4f;
    [SerializeField] private float _damageValue = 50f;
    [SerializeField] private float _knockbackForce = 15f;
    [SerializeField] private GameObject _explosionEffectPrefab;
    private Rigidbody _rigidbody;
    private IObjectPool<Bomb> _pool;
    private bool _exploded = false;
    private Collider[] _hits = new Collider[10];
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _exploded = false;
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (_exploded) return;
        _exploded = true;
        
        ExplodeDamage();
        
        GameObject effecObject = Instantiate(_explosionEffectPrefab, transform.position, Quaternion.identity);

        Despawn();
    }
    
    public void SetPool(IObjectPool<Bomb> pool)
    {
        _pool = pool;
    }
    
    public void Despawn()
    {
        if (_pool != null)
            _pool.Release(this);
        else
            Destroy(gameObject);
    }
    
    private void ExplodeDamage()
    {
        int hitCount = Physics.OverlapSphereNonAlloc(transform.position, _explosionRadius, _hits);

        for (int i = 0; i < hitCount; i++)
        {
            if (_hits[i].TryGetComponent(out IDamageable damageable))
            {
                Damage damage = new Damage()
                {
                    Value = _damageValue,
                    AttackerPosition = transform.position,
                    KnockbackForce = _knockbackForce,
                };
                damageable.TryTakeDamage(damage);
            }
        }
    }

    public void Shoot(float throwPower)
    {
        _rigidbody.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
    }
}
