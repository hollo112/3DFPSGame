using UnityEngine;
using UnityEngine.Pool;

public class BombPoolManager : MonoBehaviour
{
    public static BombPoolManager Instance { get; private set; }

    [SerializeField] private Bomb _bombPrefab;

    private IObjectPool<Bomb> _pool;
    private int _poolSize = 30;

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
        
        _pool = new ObjectPool<Bomb>(
            CreateBomb,
            OnGetBomb,
            OnReleaseBomb,
            OnDestroyBomb,
            maxSize: _poolSize
        );
    }
    
    private Bomb CreateBomb()
    {
        Bomb bomb = Instantiate(_bombPrefab);
        bomb.SetPool(_pool);
        return bomb;
    }

    private void OnGetBomb(Bomb bomb)
    {
        bomb.gameObject.SetActive(true);
    }

    private void OnReleaseBomb(Bomb bomb)
    {
        bomb.gameObject.SetActive(false);
    }

    private void OnDestroyBomb(Bomb bomb)
    {
        Destroy(bomb.gameObject);
    }

    public Bomb Get() => _pool.Get();
}
