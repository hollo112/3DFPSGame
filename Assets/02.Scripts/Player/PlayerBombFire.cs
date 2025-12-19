using UnityEngine;

public class PlayerBombFire : MonoBehaviour, IPlayerFire
{
    [SerializeField] private Transform _fireTransform;
    private PlayerStats _stats;
    private Animator _animator;
    
    public bool IsAutomatic => false;
    
    private void Awake()
    {
        _stats = GetComponent<PlayerStats>();
        _animator  = GetComponentInChildren<Animator>();
    }
    public void OnSelect() { }
    public void OnDeselect() { }
    public void Fire()
    {
        if (_stats.BombCount.Value <= 0) return;

        // Bomb bomb = BombPoolManager.Instance.Get();
        // bomb.transform.position = _fireTransform.position;
        // bomb.transform.rotation = Quaternion.identity;
        //
        // bomb.Shoot(_throwPower);
        
        _animator.SetTrigger("Throw");
        
        _stats.BombCount.Decrease(1);
    }
    public void Reload()
    {
        
    }
}
