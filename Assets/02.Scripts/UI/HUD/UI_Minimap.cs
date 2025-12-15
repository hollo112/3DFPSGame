using UnityEngine;

public class UI_Minimap : MonoBehaviour
{
    [SerializeField] private MinimapCamera _minimapCamera;

    public void OnZoomInButtonClicked()
    {
        _minimapCamera.ZoomIn();
    }

    public void OnZoomOutButtonClicked()
    {
        _minimapCamera.ZoomOut();
    }
}
