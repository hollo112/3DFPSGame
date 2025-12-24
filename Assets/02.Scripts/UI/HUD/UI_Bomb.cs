using UnityEngine.UI;
using UnityEngine;

public class UI_Bomb : MonoBehaviour
{
    [SerializeField] private Text _bombCount;
    [SerializeField] private PlayerBombFire _bombFire;
   

    void LateUpdate()
    {
        _bombCount.text = _bombFire.BombCount.ToString();
    }
}
