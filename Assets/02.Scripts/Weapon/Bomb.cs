using UnityEngine;
using UnityEngine.Pool;

public class Bomb : MonoBehaviour
{
    public GameObject _explosionEffectPrefab;
    private IObjectPool<Bomb> _pool;
    
    private void OnCollisionEnter(Collision collision)
    {
        GameObject effecObject = Instantiate(_explosionEffectPrefab);
        effecObject.transform.position = transform.position;
        
        Destroy(gameObject);
    }
    
    public void SetPool(IObjectPool<Bomb> pool)
    {
        _pool = pool;
    }
    
    public void Explode()
    {
        _pool.Release(this);
    }
}
