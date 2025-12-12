using UnityEngine;

public class CameraRecoil : MonoBehaviour
{
    [SerializeField] private CameraRotate _cameraRotate;
    private float _accumulatedRecoil = 0f; // 누적 반동값
    private float _recoverPower = 1f;
    
    private RecoilData _recoilData;
    
    private float _timer;
    private bool _isRecoiling;

    private float _recoilOffsetPitch;
    private float _recoilOffsetYaw;
    
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
            float t = Mathf.Clamp01(_timer / _recoilData.RecoilDuration);

            _recoilOffsetPitch = _recoilData.PitchCurve.Evaluate(t);
            _recoilOffsetYaw   = _recoilData.YawCurve.Evaluate(t);

            if (t >= 1f) _isRecoiling = false;
        }
        
        float deltaPitch = _cameraRotate.DeltaPitch;
        if (deltaPitch > 0f)
        {
            _accumulatedRecoil -= deltaPitch;
            
            if (_accumulatedRecoil < 0f)
                _accumulatedRecoil = 0f;
        }

        
        _accumulatedRecoil = Mathf.Lerp(_accumulatedRecoil, 0, Time.deltaTime * _recoverPower);

        float finalPitch = -(_recoilOffsetPitch + _accumulatedRecoil);
        float finalYaw   = _recoilOffsetYaw;

        transform.localRotation = Quaternion.Euler(finalPitch, finalYaw, 0);
    }
}
