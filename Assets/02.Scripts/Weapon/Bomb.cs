using UnityEngine;
using UnityEngine.Pool;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float _explosionRadius = 4f;
    [SerializeField] private float _damageValue = 10f;
    [SerializeField] private float _knockbackForce = 10f;
    public GameObject _explosionEffectPrefab;
    private Rigidbody _rigidbody;
    private IObjectPool<Bomb> _pool;
    private bool _exploded = false;

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
        Collider[] hits = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out IDamageable damageable))
            {
                Damage damage = new Damage(_damageValue, transform.position, _knockbackForce);
                damageable.TryTakeDamage(damage);
            }
        }
    }
}
