using Unity.MLAgents.Actuators;
using UnityEngine;

public class Sniper : Character
{
	public override void OnActionReceived(ActionBuffers actions)
	{
		base.OnActionReceived(actions);

		if (actions.DiscreteActions[0] == 1 && lineOfSight.VisibleTargets.Count > 0 && attackCooldown <= 0f)
		{
			GameObject target = lineOfSight.VisibleTargets[0];

			if (Attack(target, out int targetHealth))
			{
				if (targetHealth <= 0)
				{
					Debug.Log("kill");
					AddReward(10f);
					episodeManager.EndAllEpisodes();
				}	
			} 
		} else
		{
			AddReward(-0.01f);
			return;
		}

		attackCooldown = 1f / attackSpd;
	}

}
