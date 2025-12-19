using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class BulletTrail : MonoBehaviour
{
    private IObjectPool<BulletTrail> _pool;
    private TrailRenderer _trail;
    private void Awake()
    {
        _trail = GetComponent<TrailRenderer>();
    }
    
    private void OnEnable()
    {
        if (_trail != null)
            _trail.Clear();
    }
    private void OnDisable()
    {
        if (_trail != null)
            _trail.emitting = false;

        StopAllCoroutines();
    }
    public void SetPool(IObjectPool<BulletTrail> pool)
    {
        _pool = pool;
    }

    public void Play(Vector3 start, Vector3 end, float speed)
    {
        StopAllCoroutines();  
        gameObject.SetActive(true);

        StartCoroutine(MoveRoutine(start, end, speed));
    }
    
    public void ClearTrail()
    {
        _trail.Clear();
    }

    private IEnumerator MoveRoutine(Vector3 start, Vector3 end, float speed)
    {
        _trail.emitting = false;
        _trail.Clear();

        transform.position = start;

        yield return null;

        _trail.emitting = true;

        float distance = Vector3.Distance(start, end);

        float duration = distance / speed;

        float timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime / duration;
            transform.position = Vector3.Lerp(start, end, timer);
            yield return null;
        }

        yield return new WaitForSeconds(_trail.time);

        _pool.Release(this);
    }
}
