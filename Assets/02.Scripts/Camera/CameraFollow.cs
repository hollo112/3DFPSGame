using DG.Tweening;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform FirstPersonView;
    public Transform ThirdPersonView;
    public float TweenDuration = 0.5f;
    private Vector3 _currentOffset = Vector3.zero;
    private Vector3 _finalOffset;    
    private bool _isOffsetApplied = false; 
    private Tweener _tween;             
    
    [Header("Collision")]
    public float CollisionPadding = 0.2f;   // 벽에서 살짝 띄우기
    public LayerMask CollisionMask;          // Wall, Ground 등

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
        UpdateCameraPositionWithCollision();
    }

    private void UpdateCameraPositionWithCollision()
    {
        Vector3 origin = FirstPersonView.position;

        Vector3 desiredPosition = FirstPersonView.TransformPoint(_currentOffset);
        Vector3 direction = desiredPosition - origin;
        float distance = direction.magnitude;

        if (distance > 0.01f)
        {
            direction.Normalize();

            if (Physics.Raycast(origin, direction, out RaycastHit hit, distance, CollisionMask))
            {
                transform.position = hit.point - direction * CollisionPadding;
                return;
            }
        }

        transform.position = desiredPosition;
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
