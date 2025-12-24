using UnityEngine;
using UnityEngine.UI;

public class UI_OptionPopup : MonoBehaviour
{
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _exitButton;

    private void Start()
    {
        _continueButton.onClick.AddListener(OnClickContinue);
        _restartButton.onClick.AddListener(OnClickRestart);
        _exitButton.onClick.AddListener(OnClickExit);
        Hide();
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    
    private void OnClickContinue()
    {
        GameContinue();
    }
    
    private void OnClickRestart()
    {
        GameRestart();
    }
    
    private void OnClickExit()
    {
        GameExit();
    }
    
    
    private void GameContinue()
    {
        GameManager.Instance.Continue();
        Hide();
    }

    private void GameRestart()
    {
        GameManager.Instance.Restart();
    }

    private void GameExit()
    {
        GameManager.Instance.Quit();
    }
}
