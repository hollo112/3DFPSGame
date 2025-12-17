using UnityEngine;

public interface IPlayerFire
{
    public void OnSelect();
    public void OnDeselect();

    public void Fire();
    public void Reload();
    public bool IsAutomatic { get; }
}
