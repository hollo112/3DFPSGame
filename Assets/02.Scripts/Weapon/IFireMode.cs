using UnityEngine;

public interface IFireMode
{
    public bool TryFire(float damage);
    public void Reload();
}
