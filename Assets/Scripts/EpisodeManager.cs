using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class EpisodeManager : MonoBehaviour
{
	public List<Agent> agents;
	private SimpleMultiAgentGroup agentGroup;

	private void Start()
	{
		agents = new List<Agent>(gameObject.GetComponentsInChildren<Agent>());

		agentGroup = new SimpleMultiAgentGroup();

		foreach (var agent in agents)
		{
			agentGroup.RegisterAgent(agent);
		}

		foreach (var agent in agents)
			Debug.Log(agent);
	}

	public void EndAllEpisodes()
	{
		StartCoroutine(EndAllEpisodesNextFrame());
	}

	private IEnumerator EndAllEpisodesNextFrame()
	{
		yield return null; // wait one frame

		foreach (var agent in agents)
		{
			Debug.Log($"Ending episode for {agent.name}");
			agent.EndEpisode();
		}
	}
}
