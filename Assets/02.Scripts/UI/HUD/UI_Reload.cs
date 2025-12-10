using UnityEngine;
using UnityEngine.UI;

public class UI_Reload : MonoBehaviour
{
    [SerializeField] private Image _reloadBar;
    [SerializeField] private Image _reloadBarFront;
    [SerializeField] private PlayerGunFire _player;
    private void Start()
    {
        _player.OnReloadStarted += ShowReloadUI;
        _player.OnReloadProgressChanged += UpdateReloadUI;
        _player.OnReloadFinished += HideReloadUI;

        HideReloadUI();
    }
    
    private void ShowReloadUI()
    {
        _reloadBar.gameObject.SetActive(true);
        _reloadBarFront.gameObject.SetActive(true);

        _reloadBar.fillAmount = 0f;
        _reloadBarFront.fillAmount = 0f;
    }

    private void UpdateReloadUI(float progress)
    {
        _reloadBar.fillAmount = progress;
        _reloadBarFront.fillAmount = progress;
    }

    private void HideReloadUI()
    {
        _reloadBar.gameObject.SetActive(false);
        _reloadBarFront.gameObject.SetActive(false);
    }
}
