using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayingState : IState
{
    private HealthManager[] entities;
    public void OnEnter() 
    { 
        Time.timeScale = 1f; Debug.Log("Game Started");
        var roots = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var root in roots)
        {
            var ents = root.GetComponentsInChildren<HealthManager>();
            if (ents is not null && ents.Length != 0)
            {
                entities = ents;
            }
        }
        Debug.Log(roots);
    }
    public void Update() 
    {
        if (entities is null)
            return;
        foreach (var entt in entities)
        {
            if (entt.health <= 0)
            {
                GameManager.Instance.OnStopPressed();
            }
        }
    }
    public void OnExit() { }
}
