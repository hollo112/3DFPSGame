using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerStats : MonoBehaviour
{
    [SerializeField] private PlayerStats _stats;
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private Slider _staminaSlider;
    [SerializeField] private Text _bombCount;
    private void Update()
    {
        _healthSlider.value = _stats.Health.Value / _stats.Health.MaxValue;
        _staminaSlider.value = _stats.Stamina.Value /  _stats.Stamina.MaxValue;

        _bombCount.text = _stats.BombCount.Value.ToString();
    }
}
