using UnityEngine;

public class PlayerBombFire : MonoBehaviour, IPlayerFire
{
    [SerializeField] private Transform _fireTransform;
    [SerializeField] private float _throwPower = 15f;
    private PlayerStats _stats;
    public bool IsAutomatic => false;
    
    private void Awake()
    {
        _stats = GetComponent<PlayerStats>();
    }
    public void OnSelect() { }
    public void OnDeselect() { }
    public void Fire()
    {
        if (_stats.BombCount.Value <= 0) return;

        Bomb bomb = BombPoolManager.Instance.Get();
        bomb.transform.position = _fireTransform.position;
        bomb.transform.rotation = Quaternion.identity;

        bomb.Shoot(_throwPower);

        _stats.BombCount.Decrease(1);
    }
    public void Reload()
    {
        
    }
}
