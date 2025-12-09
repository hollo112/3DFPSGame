using UnityEngine;

// 목표를 따라다니는 카메라
public class CameraFollow : MonoBehaviour
{
    public Transform FirstPersonView;
    public Transform ThirdPersonView;
    private Transform _cameraTransform;
    private bool _isFirstPersonView;
    private void LateUpdate()
    {
        UpdateCameraPosition();
    }

    private void Start()
    {
        _isFirstPersonView = true;
        _cameraTransform = FirstPersonView;
    }
    private void Update()
    {
        ChangeView();
    }

    private void ChangeView()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            _isFirstPersonView = !_isFirstPersonView;
            _cameraTransform.position = _isFirstPersonView ? FirstPersonView.position : ThirdPersonView.position;
        }
    }

    private void UpdateCameraPosition()
    {
        transform.position = _cameraTransform.position;
    }
}
