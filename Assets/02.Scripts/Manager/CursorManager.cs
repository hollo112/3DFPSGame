using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private static CursorManager _instance;
    public static CursorManager Instance => _instance;
    
    private bool _isLocked = true;
    public bool IsLocked => _isLocked;
    void Awake()
    {
        _instance = this;
        
        LockCursor();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursor();
        }
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _isLocked = true;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _isLocked = false;
    }
    
    private void ToggleCursor()
    {
        if (_isLocked)
            UnlockCursor();
        else
            LockCursor();
    }
}
