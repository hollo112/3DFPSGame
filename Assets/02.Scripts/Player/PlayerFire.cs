using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    [SerializeField] private Transform _fireTransform;
    [SerializeField] private Bomb _bombPrefab;
    [SerializeField] private float _throwPower = 15f;
    private PlayerStats _stats;
    
    private void Start()
    {
        _stats = GetComponent<PlayerStats>();
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (_stats.BombCount.Value <= 0) return;
            
            Bomb bomb = Instantiate(_bombPrefab, _fireTransform.position, Quaternion.identity);
            Rigidbody rigidbody = bomb.GetComponent<Rigidbody>();
            rigidbody.AddForce(Camera.main.transform.forward * _throwPower, ForceMode.Impulse);
            
            _stats.BombCount.Decrease(1);
        }
    }
}
