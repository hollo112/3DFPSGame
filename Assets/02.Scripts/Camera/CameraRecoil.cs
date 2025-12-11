using UnityEngine;

public class CameraRecoil : MonoBehaviour
{
    private float _accumulatedRecoil = 0f; // 누적 반동값
    private RecoilData _recoilData;
    
    private float _timer;
    private bool _isRecoiling;

    private float _pitchOffset;
    private float _yawOffset;

    public void PlayRecoil(RecoilData recoilData)
    {
        _recoilData = recoilData;
        _timer = 0f;
        _isRecoiling = true;
        _accumulatedRecoil += _recoilData.AccumulationStrength * _recoilData.RecoilPower;
    }

    private void Update()
    {
        if (_isRecoiling)
        {
            _timer += Time.deltaTime;
            float recoilTime = _timer / _recoilData.RecoilDuration;
            if (recoilTime >= 1f)
            {
                _isRecoiling = false;
                recoilTime = 1f;
            }

            _pitchOffset = _recoilData.PitchCurve.Evaluate(recoilTime);
            _yawOffset   = _recoilData.YawCurve.Evaluate(recoilTime);
        }

        float finalPitch = -(_pitchOffset + _accumulatedRecoil);
        float finalYaw   = _yawOffset;

        transform.localRotation = Quaternion.Euler(finalPitch, finalYaw, 0);
    }
}
