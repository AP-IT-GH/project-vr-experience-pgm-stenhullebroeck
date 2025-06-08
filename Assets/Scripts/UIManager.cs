using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindFirstObjectByType<UIManager>();
            return instance;

        }
    }

    public GameObject startCanvas;
    public GameObject gameplayCanvas;
    public GameObject pauseCanvas;
    public GameObject gameOverCanvas;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
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
