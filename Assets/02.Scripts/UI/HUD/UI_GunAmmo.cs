using UnityEngine;
using UnityEngine.UI;

public class UI_GunAmmo : MonoBehaviour
{
    [SerializeField] private Magazine _magazine;
    [SerializeField] private Text _reserveAmmo;
    [SerializeField] private Text _magazineSize;
    void Update()
    {
        _reserveAmmo.text = _magazine.ReserveAmmo.ToString();
        _magazineSize.text = _magazine.MagzineSize.ToString();
    }
}
