using UnityEngine;
using UnityEngine.Pool;

public class BulletTrailPoolManager : MonoBehaviour
{
    public static BulletTrailPoolManager Instance { get; private set; }

    [SerializeField] private BulletTrail _bulletTrailPrefab;
    [SerializeField] private int _poolSize = 60;

    private IObjectPool<BulletTrail> _pool;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        _pool = new ObjectPool<BulletTrail>(
            CreateTrail,
            OnGetTrail,
            OnReleaseTrail,
            OnDestroyTrail,
            maxSize: _poolSize
        );
    }

    private BulletTrail CreateTrail()
    {
        BulletTrail trail = Instantiate(_bulletTrailPrefab);
        trail.SetPool(_pool);
        return trail;
    }

    private void OnGetTrail(BulletTrail trail)
    {
        trail.gameObject.SetActive(true);
        trail.ClearTrail();
    }

    private void OnReleaseTrail(BulletTrail trail)
    {
        trail.gameObject.SetActive(false);
    }

    private void OnDestroyTrail(BulletTrail trail)
    {
        Destroy(trail.gameObject);
    }

    public BulletTrail Get()
    {
        return _pool.Get();
    }
}
