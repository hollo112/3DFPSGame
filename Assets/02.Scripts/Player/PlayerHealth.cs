using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    private PlayerStats _stats;

    private void Awake()
    {
        _stats = GetComponent<PlayerStats>();
    }
    
    public bool TryTakeDamage(Damage damage)
    {
        // Todo. 플레이어 죽었을때 혹은 데미지를 입으면 안될때 false
        _stats.Health.ConsumeClamped(damage.Value);
        if (_stats.Health.Value <= 0)
        {
            GameManager.Instance.GameOver();
        }
        
        return true;
    }
}
