using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private StateMachine _stateMachine;

    private bool startPressed;
    private bool pausePressed;
    private bool stopPressed;
    private bool mainMenuPressed;

    private InputAction pauseAction;
    private bool prevPressedPause = false;

    void Awake()
    {
        if (Instance == null) Instance = this; else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        var start = new StartState();
        var playing = new PlayingState();
        var paused = new PausedState();
        var gameOver = new GameOverState();

        _stateMachine = new StateMachine();
        _stateMachine.AddTransition(start, playing, () => startPressed);
        _stateMachine.AddTransition(playing, paused, () => pausePressed);
        _stateMachine.AddTransition(paused, playing, () => pausePressed);
        _stateMachine.AddTransition(paused, start, () => mainMenuPressed);
        _stateMachine.AddTransition(playing, gameOver, () => stopPressed);
        _stateMachine.AddTransition(paused, gameOver, () => stopPressed);
        _stateMachine.AddTransition(start, gameOver, () => stopPressed);

        _stateMachine.SetState(start);
    }

    private void Start()
    {
        pauseAction = InputSystem.actions.FindAction("Pause");
    }

    void Update()
    {
        if (!prevPressedPause)
        {
            pausePressed = pauseAction.ReadValue<float>() > 0f;
            prevPressedPause = true;
        }
        else if (pauseAction.ReadValue<float>() == 0f)
            prevPressedPause = false;
        _stateMachine.Tick();
        startPressed = pausePressed = stopPressed = mainMenuPressed = false;
    }

    public void OnStartPressed() => startPressed = true;
    public void OnPausePressed() => pausePressed = true;
    public void OnStopPressed() => stopPressed = true;
    public void OnMainMenuPressed() => mainMenuPressed = true;

    public void SelectLevel(int scene)
    {
        SceneManager.LoadScene(scene);
        OnStartPressed();
    }


}
