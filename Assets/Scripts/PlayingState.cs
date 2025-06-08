using UnityEngine;

public class PlayingState : IState
{
    public void OnEnter() { Time.timeScale = 1f; Debug.Log("Game Started"); }
    public void Update() { }
    public void OnExit() { }
}
