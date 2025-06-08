using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausedState : IState
{
    public delegate void OnPause();
    public static event OnPause onPause;

    public delegate void OnResume();
    public static event OnResume onResume;

    public void OnEnter()
    {
        Debug.Log("Game is paused");
        onPause?.Invoke();
        Time.timeScale = 0f;
    }
        public void Update()
    {
    }

    public void OnExit()
    {
        Debug.Log("Resuming Game");
        onResume?.Invoke();
        Time.timeScale = 1f;
    }
}