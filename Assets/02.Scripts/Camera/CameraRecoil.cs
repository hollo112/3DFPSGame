using UnityEngine;

public class CameraRecoil : MonoBehaviour
{
    [SerializeField] private Transform _cameraPivot;
    [SerializeField] private float _recoilAngle = 2f;
    [SerializeField] private float _recoverySpeed = 10f;

    private float _currentRecoil;

    private void Update()
    {
        _cameraPivot.localRotation = Quaternion.Euler(-_currentRecoil, 0, 0);
    }

    public void ApplyRecoil(float power)
    {
        _currentRecoil += _recoilAngle * power;
    }
}
