using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject startCanvas;
    public GameObject gameplayCanvas;
    public GameObject pauseCanvas;
    public GameObject gameOverCanvas;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowStartCanvas() => startCanvas.SetActive(true);
    public void HideStartCanvas() => startCanvas.SetActive(false);
    public void ShowGameplayCanvas() => gameplayCanvas.SetActive(true);
    public void HideGameplayCanvas() => gameplayCanvas.SetActive(false);
    public void ShowPauseCanvas() => pauseCanvas.SetActive(true);
    public void HidePauseCanvas() => pauseCanvas.SetActive(false);
    public void ShowGameOverCanvas() => gameOverCanvas.SetActive(true);
    public void HideGameOverCanvas() => gameOverCanvas.SetActive(false);
}
