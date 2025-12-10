using UnityEngine;
using UnityEngine.UI;

public class UI_GunAmmo : MonoBehaviour
{
    [SerializeField] private PlayerGunFire _playerGunFire;
    [SerializeField] private Text _reserveAmmo;
    [SerializeField] private Text _magazineSize;
    void Update()
    {
        _reserveAmmo.text = _playerGunFire.ReserveAmmo.ToString();
        _magazineSize.text = _playerGunFire.MagzineSize.ToString();
    }
}
