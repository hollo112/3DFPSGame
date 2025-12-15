using System;
using System.Collections;
using UnityEngine;

public class Drum : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject _explosionEffectPrefab;
    [SerializeField] private ConsumableStat _health;
    [SerializeField] private float _damageValue = 50f;
    [SerializeField] private float _danageTime = 0.1f;
    [SerializeField] private float _destroyTime = 4f;
    [SerializeField] private float _knockbackForce = 15f;
    [SerializeField] private float _explosionRadius = 4f;
    [SerializeField] private float _explosionForce = 15f;
    [SerializeField] private float _rotationForce = 5f;
    private Rigidbody _rigidbody;
    private bool _isExploded = false;
    private Collider[] _hits = new Collider[10];
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public bool TryTakeDamage(Damage damage)
    {
        if (_isExploded)
        {
            return false;
        }
        
        _health.ConsumeClamped(damage.Value);

        if (_health.Value <= 0)
        {
            _isExploded = true;
            StartCoroutine(Explose_Coroutine());
        }

        return true;
    }

    private IEnumerator Explose_Coroutine()
    {
        GameObject effecObject = Instantiate(_explosionEffectPrefab, transform.position, Quaternion.identity);
        _rigidbody.AddForce(Vector3.up * _explosionForce, ForceMode.Impulse);
        Vector3 randomTorque = UnityEngine.Random.insideUnitSphere * _rotationForce;
        _rigidbody.AddTorque(randomTorque, ForceMode.Impulse);

        yield return new WaitForSeconds(_danageTime);
        
        ExplodeDamage();
        
        yield return new WaitForSeconds(_destroyTime);
        
        Destroy(gameObject);
    }
    
    private void ExplodeDamage()
    {
        int hitCount = Physics.OverlapSphereNonAlloc(transform.position, _explosionRadius, _hits);

        for (int i = 0; i < hitCount; i++)
        {
            if (_hits[i].TryGetComponent(out IDamageable damageable))
            {
                Damage damage = new Damage(_damageValue, transform.position, _knockbackForce);
                damageable.TryTakeDamage(damage);
            }
        }
    }
}
