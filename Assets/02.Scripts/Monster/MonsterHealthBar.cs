using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Monster))]
public class MonsterHealthBar : MonoBehaviour
{
    private MonsterStats _monsterStats;
    [SerializeField] private Image _guageimage;
    [SerializeField] private Transform _healthbarTransform;
    private float _lastHealth = -1f;
    private void Awake()
    {
        _monsterStats = GetComponent<MonsterStats>();
    }

    private void LateUpdate()
    {
        if (_lastHealth != _monsterStats.Health.Value)
        {
            _lastHealth = _monsterStats.Health.Value;
            _guageimage.fillAmount = _monsterStats.Health.Value / _monsterStats.Health.MaxValue;
        }
        
        // 빌보드 기법 : 카메라의 위치와 회전에 상관없이 항상 정면을 바라보게 하는 기법
        _healthbarTransform.forward = Camera.main.transform.forward;
    }
}
