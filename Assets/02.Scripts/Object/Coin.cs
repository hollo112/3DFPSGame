using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Rigidbody _rigidBody;
    private Collider _collider;
    private Transform _player;
    
    [Header("Throw Power")]
    [SerializeField] private float _minPower = 3f;
    [SerializeField] private float _maxPower = 6f;
    [SerializeField] private float _upForce = 0.2f;
    [Header("이동")]
    [SerializeField] private float _attractDelay = 1.5f;
    [SerializeField] private float _flyDuration = 0.4f;
    [SerializeField] private float _bezierHeight = 1.2f;
    
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        ThrowRandom();
        StartCoroutine(AttractToPlayerRoutine());
    }
    
    public void Throw(Vector3 force)
    {
        _rigidBody.linearVelocity = Vector3.zero;
        _rigidBody.angularVelocity = Vector3.zero;

        _rigidBody.isKinematic = false;

        _rigidBody.AddForce(force, ForceMode.Impulse);
    }
    
    private void ThrowRandom()
    {
        Vector2 direction2D = Random.insideUnitCircle.normalized;
        Vector3 direction = new Vector3(direction2D.x, _upForce, direction2D.y);

        float power = Random.Range(_minPower, _maxPower);

        Throw(direction * power);
    }
    
    private IEnumerator AttractToPlayerRoutine()
    {
        yield return new WaitForSeconds(_attractDelay);

        // 물리 비활성화
        _rigidBody.isKinematic = true;
        _collider.enabled = false;

        Vector3 start = transform.position;
        Vector3 end = _player.position;

        // 베지어 중간 제어점 (위로 살짝)
        Vector3 control = (start + end) * 0.5f + Vector3.up * _bezierHeight;

        float timer = 0f;

        while (timer < _flyDuration)
        {
            timer += Time.deltaTime;
            float t = timer / _flyDuration;

            transform.position = CalculateBezier(start, control, end, t);
            yield return null;
        }

        // 도착 처리
        Collect();
    }
    
    private Vector3 CalculateBezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float u = 1f - t;
        return u * u * p0 + 2f * u * t * p1 + t * t * p2;
    }

    private void Collect()
    {
        // TODO: 코인 획득 처리 
        Destroy(gameObject);
    }
}
