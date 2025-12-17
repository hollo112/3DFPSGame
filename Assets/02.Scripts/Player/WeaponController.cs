using System;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private MonoBehaviour[] _weaponBehaviours;
    private IPlayerFire[] _weapons;
    
    private int _currentIndex;
    private IPlayerFire _currentWeapon;
    public event Action<IPlayerFire> OnWeaponChanged;
    private void Awake()
    {
        _weapons = new IPlayerFire[_weaponBehaviours.Length];
        for (int i = 0; i < _weapons.Length; i++)
        {
            if(_weaponBehaviours[i] is IPlayerFire)
                _weapons[i] = (IPlayerFire)_weaponBehaviours[i];
        }

        Equip(0);
    }
    
    private void Update()
    {
        if (GameManager.Instance.State != EGameState.Playing) return;
        if (!CursorManager.Instance.IsLocked) return;
        
        float wheel = Input.mouseScrollDelta.y;
        if (wheel != 0)
        {
            int next = wheel > 0 ? 1 : -1;
            Equip((_currentIndex + next + _weapons.Length) % _weapons.Length);
        }

        if (_currentWeapon.IsAutomatic)
        {
            if (Input.GetMouseButton(0))
                _currentWeapon.Fire();
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
                _currentWeapon.Fire();
        }

        if (Input.GetKeyDown(KeyCode.R))
            _currentWeapon.Reload();
    }

    private void Equip(int index)
    {
        _currentWeapon?.OnDeselect();

        _currentIndex = index;
        _currentWeapon = _weapons[_currentIndex];
        _currentWeapon.OnSelect();
        
        OnWeaponChanged?.Invoke(_currentWeapon);
    }
}
