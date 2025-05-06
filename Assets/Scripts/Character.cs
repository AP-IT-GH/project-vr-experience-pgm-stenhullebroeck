using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEngine.InputSystem;
using System;

public class Character : Agent
{
	[SerializeField]
    protected int health;
	[SerializeField]
	protected int damage;
	[SerializeField]
    protected float movementSpd = 5;
	[SerializeField]
	protected float attackSpd;
	[SerializeField]
	protected float attackRad;
	[SerializeField]
	protected float rotationSpd = 90f;

	InputAction moveAction;

    void Start()
    {
		moveAction = InputSystem.actions.FindAction("Move");
    }

    void Update()
    {
        
    }

	public override void OnActionReceived(ActionBuffers actions)
	{
		Vector3 controller = Vector3.zero;


		controller.x = actions.ContinuousActions[0];
		controller.z = actions.ContinuousActions[1];

		transform.Translate(Vector3.forward * controller.z * this.movementSpd * Time.deltaTime);
		transform.Rotate(Vector3.up, controller.x * rotationSpd * Time.deltaTime, Space.Self);

		//if (transform.localPosition.y < 0)
		//{
		//	AddReward(-0.5f);
		//	EndEpisode();
		//}  
	}

	public override void Heuristic(in ActionBuffers actionsOut)
	{
		var continuousActions = actionsOut.ContinuousActions;

		Vector2 moveValue = moveAction.ReadValue<Vector2>();

		continuousActions[0] = moveValue.x;
		continuousActions[1] = moveValue.y;

	}
}
