using UnityEngine;

public class GameOverState : IState
{
    public void OnEnter()
    {
        Debug.Log("Game Over");
        Time.timeScale = 0f;
        UIManager.Instance.ShowGameOverCanvas();
        var targetPos = GameObject.FindWithTag("UIOffset").transform;
        UIManager.Instance.gameOverCanvas.transform.SetPositionAndRotation(targetPos.position, targetPos.rotation);
    }

    public void Update() { }

    public void OnExit()
    {
        UIManager.Instance.HideGameOverCanvas();
    }
}
