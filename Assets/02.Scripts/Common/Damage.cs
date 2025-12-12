using UnityEngine;

public struct Damage
{
    public float Value;
    public Vector3 AttackerPosition;
    
    public Damage(float value, Vector3 attackerPosition)
    {
        Value = value;
        AttackerPosition = attackerPosition;
    }
}
