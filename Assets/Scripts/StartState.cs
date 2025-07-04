using UnityEngine;
using UnityEngine.SceneManagement;

public class StartState : IState
{
    public void OnEnter()
    {
        Debug.Log("Entering StartState");
        if (SceneManager.GetActiveScene().buildIndex != 0)
            SceneManager.LoadScene(0);

        Time.timeScale = 0f;

        UIManager.Instance.ShowStartCanvas();

        //UIManager.Instance.HideGameplayCanvas();
        UIManager.Instance.HidePauseCanvas();
        UIManager.Instance.HideGameOverCanvas();

        //ArenaLoader.Instance.PrepareSelectedArena();
    }

    public void Update()
    {
    }

    public void OnExit()
    {
        Debug.Log("Exiting StartState");
        UIManager.Instance.HideStartCanvas();
    }
}
