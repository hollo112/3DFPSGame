using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject _explosionEffectPrefab;
    
    private void OnCollisionEnter(Collision collision)
    {
        GameObject effecObject = Instantiate(_explosionEffectPrefab);
        effecObject.transform.position = transform.position;
        
        Destroy(gameObject);
    }
}
