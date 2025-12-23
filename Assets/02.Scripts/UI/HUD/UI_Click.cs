using TMPro;
using UnityEngine;

public class UI_Click : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _leftClickCountTextUI;
    [SerializeField] private TextMeshProUGUI _rightClickCountTextUI;

    private void Start()
    {
        Refresh();
        
        ClickManager.Instance.OnDataChanged += Refresh;
    }
    
    private void Refresh()
    {
        _leftClickCountTextUI.text = $"왼쪽 클릭 : {ClickManager.Instance.LeftClickCount}번";
        _rightClickCountTextUI.text = $"오른쪽 클릭 : {ClickManager.Instance.RightClickCount}번";
    }
}
