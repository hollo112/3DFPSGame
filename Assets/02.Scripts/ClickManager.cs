using System;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    public static ClickManager Instance;
    
    private int _leftClickCount;
    private int _rightClickCount;
    
    public int LeftClickCount => _leftClickCount;
    public int RightClickCount => _rightClickCount;
    
    public event Action OnDataChanged;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _leftClickCount++;
            OnDataChanged?.Invoke();
        }

        if (Input.GetMouseButtonDown(1))
        {
            _rightClickCount++;
            OnDataChanged?.Invoke();
        }
    }
}
