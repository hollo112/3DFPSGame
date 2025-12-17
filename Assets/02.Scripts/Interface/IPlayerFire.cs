using UnityEngine;

public interface IPlayerFire
{
    public void OnSelect();
    public void OnDeselect();

    public void Fire();
    public void Reload();
    bool IsAutomatic { get; }
}
