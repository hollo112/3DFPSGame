using UnityEngine;

public class PlayerBombFire : MonoBehaviour, IPlayerFire
{
    [SerializeField] private Transform _fireTransform;
    [SerializeField] private int _maxBombCount = 5;
    private int _bombCount;
    public int BombCount => _bombCount;

    private PlayerStats _stats;
    private Animator _animator;
    
    public bool IsAutomatic => false;
    
    private void Awake()
    {
        _stats = GetComponent<PlayerStats>();
        _animator  = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        _bombCount = _maxBombCount;
    }
    public void OnSelect() { }
    public void OnDeselect() { }
    public void Fire()
    {
        if (_bombCount <= 0) return;

        // Bomb bomb = BombPoolManager.Instance.Get();
        // bomb.transform.position = _fireTransform.position;
        // bomb.transform.rotation = Quaternion.identity;
        //
        // bomb.Shoot(_throwPower);
        
        _animator.SetTrigger("Throw");

        _bombCount--;
    }
    public void Reload()
    {
        
    }
}
