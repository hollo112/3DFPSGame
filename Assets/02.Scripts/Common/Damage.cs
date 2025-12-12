using UnityEngine;

public struct Damage
{
    public float Value;
    public Vector3 AttackerPosition;
    public float KnockbackForce;
    public Damage(float value, Vector3 attackerPosition, float knockbackForce = 4f)
    {
        Value = value;
        AttackerPosition = attackerPosition;
        KnockbackForce = knockbackForce;
    }
}
