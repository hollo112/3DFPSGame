using DG.Tweening;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform FirstPersonView;
    public Transform ThirdPersonView;
    public Transform Top;
    public float TweenDuration = 0.5f;
    private Vector3 _currentOffset = Vector3.zero;
    private Vector3 _firstPersonOffset;  
    private Vector3 _thirdPersonOffset;    
    private Vector3 _topViewOffset;    
    private Tweener _tween;             
    private const float CollisionCheckDistance = 0.01f;
    [Header("Collision")]
    public float CollisionPadding = 0.2f;   // 벽에서 살짝 띄우기
    public LayerMask CollisionMask;          // Wall, Ground 등

    private void Start()
    {
        _firstPersonOffset = Vector3.zero;
        _thirdPersonOffset = FirstPersonView.InverseTransformPoint(ThirdPersonView.position);
        _topViewOffset =  FirstPersonView.InverseTransformPoint(Top.position);
        transform.position = FirstPersonView.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            GameManager.Instance.CycleCameraMode();
            ChangeView(GameManager.Instance.ViewMode);
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

        if (distance > CollisionCheckDistance)
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

    private void ChangeView(ECameraViewMode cameraViewMode)
    {
        Vector3 targetPosition = Vector3.zero;
        
        switch (cameraViewMode)
        {
            case ECameraViewMode.FirstPerson:
                targetPosition = _firstPersonOffset;
                break;
            case ECameraViewMode.ThirdPerson:
                targetPosition = _thirdPersonOffset;
                break;
            case ECameraViewMode.Top:
                targetPosition = _topViewOffset;
                break;
        }

        _tween = DOTween.To(
            () => _currentOffset,
            x => _currentOffset = x,
            targetPosition,
            TweenDuration
        ).SetEase(Ease.InOutQuad);
    }
}
