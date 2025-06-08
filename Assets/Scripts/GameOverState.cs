using UnityEngine;

public class GameOverState : IState
{
    public void OnEnter()
    {
        Debug.Log("Game Over");
        Time.timeScale = 0f;
        UIManager.Instance.ShowGameOverCanvas();
    }

    public void Update() { }

    public void OnExit()
    {
        UIManager.Instance.HideGameOverCanvas();
    }
}
