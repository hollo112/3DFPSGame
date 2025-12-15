using UnityEngine;

public class PlayerBombFire : MonoBehaviour
{
    [SerializeField] private Transform _fireTransform;
    [SerializeField] private float _throwPower = 15f;
    private PlayerStats _stats;
    
    private void Start()
    {
        _stats = GetComponent<PlayerStats>();
    }
    
    private void Update()
    {
        if (GameManager.Instance.State != EGameState.Playing) return;
        
        if (Input.GetMouseButtonDown(1))
        {
            if (_stats.BombCount.Value <= 0) return;
            
            Bomb bomb = BombPoolManager.Instance.Get();
            
            bomb.transform.position = _fireTransform.position;
            bomb.transform.rotation = Quaternion.identity;
            
            Rigidbody rigidbody = bomb.GetComponent<Rigidbody>();
            rigidbody.AddForce(Camera.main.transform.forward * _throwPower, ForceMode.Impulse);
            
            //_stats.BombCount.Decrease(1);
        }
    }
}
