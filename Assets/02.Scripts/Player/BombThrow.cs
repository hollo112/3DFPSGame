using UnityEngine;

public class PlayerBombThrow : MonoBehaviour
{
    [SerializeField] private Transform _fireTransform;
    [SerializeField] private float _throwPower = 15f;
    public void Throw()
    {
        Bomb bomb = BombPoolManager.Instance.Get();
        bomb.transform.position = _fireTransform.position;
        bomb.transform.rotation = Quaternion.identity;

        bomb.Shoot(_throwPower);
    }
}
