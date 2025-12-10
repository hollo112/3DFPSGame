using DG.Tweening;
using UnityEngine;

// 목표를 따라다니는 카메라
public class CameraFollow : MonoBehaviour
{
    public Transform FirstPersonView;
    public Transform ThirdPersonView;
    public float TweenDuration = 0.5f;
    private Vector3 _currentOffset = Vector3.zero;
    private Vector3 _finalOffset;    
    private bool _isOffsetApplied = false; 
    private Tweener _tween;              

    private void Start()
    {
        _finalOffset = FirstPersonView.InverseTransformPoint(ThirdPersonView.position);
        transform.position = FirstPersonView.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ChangeView();
        }
    }

    private void LateUpdate()
    {
        transform.position = FirstPersonView.TransformPoint(_currentOffset);
    }

    private void ChangeView()
    {
        if (!_isOffsetApplied)
        {
            _tween = DOTween.To(
                    () => _currentOffset,
                    x => _currentOffset = x,
                    _finalOffset,
                    TweenDuration
                ).SetEase(Ease.InOutQuad)
                .OnComplete(() => _isOffsetApplied = true);
        }
        else
        {
            _tween = DOTween.To(
                    () => _currentOffset,
                    x => _currentOffset = x,
                    Vector3.zero,
                    TweenDuration
                ).SetEase(Ease.InOutQuad)
                .OnComplete(() => _isOffsetApplied = false);
        }
    }
}
