using UnityEngine;
using UnityEngine.InputSystem.HID;

public class UI_Minimap : MonoBehaviour
{
    [SerializeField] private MinimapCamera _minimapCamera;

    public void OnZoomInButtonClicked()
    {
        Debug.Log("OnZoomInButtonClicked");
        _minimapCamera.ZoomIn();
    }

    public void OnZoomOutButtonClicked()
    {
        Debug.Log("OnZoomOutButtonClicked");
        _minimapCamera.ZoomOut();
    }
}
