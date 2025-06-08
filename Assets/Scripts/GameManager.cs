
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private StateMachine _stateMachine;

    private bool startPressed;
    private bool pausePressed;
    private bool stopPressed;

    public GameObject outsideArena; //SmallerTerrain
    public GameObject insideArena; //GameField
    private GameObject activeArena;

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
        _stateMachine.AddTransition(playing, gameOver, () => stopPressed);
        _stateMachine.AddTransition(paused, gameOver, () => stopPressed);
        _stateMachine.AddTransition(start, gameOver, () => stopPressed);

        _stateMachine.SetState(start);
    }

    void Update()
    {
        _stateMachine.Tick();
        startPressed = pausePressed = stopPressed = false;
    }

    public void OnStartPressed() => startPressed = true;
    public void OnPausePressed() => pausePressed = true;
    public void OnStopPressed() => stopPressed = true;


    public void StartInsideArena()
    {
        Debug.Log("StartInsideArena called");
        SelectArena(false, true);
        OnStartPressed();
    }

    public void StartOutsideArena()
    {
        SelectArena(true, false);
        OnStartPressed();
    }

    private void SelectArena(bool useOutside, bool useInside)
    {
        if (activeArena != null)
            activeArena.SetActive(false);

        if (useOutside)
        activeArena = outsideArena;
        else if (useInside)
            activeArena = insideArena;
        else
        {
            Debug.LogWarning("Error with Booleans, default to insideArena");
            activeArena = insideArena;
        }  

        activeArena.SetActive(true);
    }


}
