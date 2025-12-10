using UnityEngine;

public class CameraRecoil : MonoBehaviour
{
    [Header("반동 패턴 세팅")]
    [SerializeField] private AnimationCurve _pitchCurve;
    [SerializeField] private AnimationCurve _yawCurve;
    [SerializeField] private float _recoilDuration = 0.1f;
    [SerializeField] private float _accumulationStrength = 0.2f;
    private float _accumulatedRecoil = 0f; // 누적 반동값
    
    [Header("카메라")]
    [SerializeField] private Transform _cameraPivot;

    private float _timer;
    private bool _isRecoiling;

    private float _pitchOffset;
    private float _yawOffset;

    public void PlayRecoil(float power)
    {
        _timer = 0f;
        _isRecoiling = true;
        _accumulatedRecoil += _accumulationStrength * power;
    }

    private void Update()
    {
        if (_isRecoiling)
        {
            _timer += Time.deltaTime;
            float recoilTime = _timer / _recoilDuration;
            if (recoilTime >= 1f)
            {
                _isRecoiling = false;
                recoilTime = 1f;
            }

            _pitchOffset = _pitchCurve.Evaluate(recoilTime);
            _yawOffset   = _yawCurve.Evaluate(recoilTime);
        }

        float finalPitch = -(_pitchOffset + _accumulatedRecoil);
        float finalYaw   = _yawOffset;

        _cameraPivot.localRotation = Quaternion.Euler(finalPitch, finalYaw, 0);
    }
}
