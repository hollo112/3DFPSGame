using UnityEngine;

public class Magazine : MonoBehaviour
{
    public int MagazineMaxSize = 30;
    private int _magazineSize = 30;
    public int MagzineSize => _magazineSize;
    public int MagazineCount = 5;
    private int _reserveAmmo;
    public int ReserveAmmo => _reserveAmmo;
    
    private void Start()
    {
        _reserveAmmo = MagazineMaxSize * (MagazineCount - 1);
    }
    
    public void Reload()
    {
        int need = MagazineMaxSize - _magazineSize;
        if (need <= 0) return;
        if (_reserveAmmo <= 0) return;

        int take = Mathf.Min(need, _reserveAmmo);
        _magazineSize += take;
        _reserveAmmo -= take;
    }

    public void DecreaseMagazineSize(int amount)
    {
        _magazineSize -= amount;
    }
}
