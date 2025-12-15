using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public ConsumableStat Health;
    public ConsumableStat Stamina;
    public ConsumableStat BombCount;
    
    public ValueStat Damage;
    public ValueStat WalkSpeed;
    public ValueStat RunSpeed;
    public ValueStat JumpPower;

    private void Start()
    {
        Health.Initialize();
        Stamina.Initialize();
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        Health.Regenerate(deltaTime);
        Stamina.Regenerate(deltaTime);
    }
    
}
