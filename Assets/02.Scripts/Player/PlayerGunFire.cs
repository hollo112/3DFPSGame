using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerGunFire : MonoBehaviour
{
    [SerializeField] private RayGun rayGun;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            rayGun.Reload();

        if (Input.GetMouseButton(0))
        {
            rayGun.TryFire();
        }
    }
}