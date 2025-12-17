using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;
    
    private EGameState _state = EGameState.Ready;
    public EGameState State => _state;
    
    private ECameraViewMode _viewMode = ECameraViewMode.FirstPerson;
    public ECameraViewMode ViewMode => _viewMode;
    
    [SerializeField] private TextMeshProUGUI _stateTextUI;
    [SerializeField] private Image _crosshairImage;

    private void Awake()
    {
        _instance = this;
    }
    
    private void Start()
    {
        _crosshairImage.gameObject.SetActive(false);
        _stateTextUI.gameObject.SetActive(true);
        
        _state = EGameState.Ready;
        _stateTextUI.text = "준비중...";

        StartCoroutine(StartToPlay_Coroutine());
    }

    private IEnumerator StartToPlay_Coroutine()
    {
        yield return new WaitForSeconds(2f);
        
        _stateTextUI.text = "시작!";
        
        yield return new WaitForSeconds(0.5f);
        
        _state = EGameState.Playing;
        _crosshairImage.gameObject.SetActive(true);
        _stateTextUI.gameObject.SetActive(false);
    }

    public void GameOver()
    {
        _state = EGameState.GameOver;
        _crosshairImage.gameObject.SetActive(false);
        _stateTextUI.gameObject.SetActive(true);
        
        _stateTextUI.text = "게임오버";
    }

    public void CycleCameraMode()
    {
        int next = ((int)_viewMode + 1) % System.Enum.GetValues(typeof(ECameraViewMode)).Length;
        _viewMode = (ECameraViewMode)next;
    }
}
