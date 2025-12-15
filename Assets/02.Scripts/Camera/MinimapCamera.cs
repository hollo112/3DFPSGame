using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MinimapCamera : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _offsetY = 10f;
    [SerializeField] private float _zoomOffset = 2f;
    [SerializeField] private float _minZoom = 5f;
    [SerializeField] private float _maxZoom = 13f;
    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }
    
    private void LateUpdate()
    {
        Vector3 targetPosition = _target.position;
        Vector3 finalPosition = targetPosition + new Vector3(0, _offsetY, 0f);
        
        transform.position = finalPosition;
        
        transform.rotation = Quaternion.Euler(90f, _target.eulerAngles.y, 0f);
    }

    public void ZoomIn()
    {
        _camera.orthographicSize = Mathf.Max(_minZoom, _camera.orthographicSize - _zoomOffset);
    }

    public void ZoomOut()
    {
        _camera.orthographicSize = Mathf.Min(_maxZoom, _camera.orthographicSize + _zoomOffset);
    }
}
