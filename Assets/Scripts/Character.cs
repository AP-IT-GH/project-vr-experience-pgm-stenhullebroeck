using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class Character : Agent
{
	[SerializeField]
    protected int health;
	[SerializeField]
	protected int damage;
	[SerializeField]
    protected float movementSpd = 5;
	[SerializeField]
	protected float rotationSpd = 90f;

	[Header("Attack")]
	public LayerMask obstacleMask;
	public LayerMask targetMask;
	[SerializeField]
	protected float attackSpd;
	[SerializeField]
	protected float attackRad;
	[SerializeField]
	[Range(0, 1)]
	protected float accuracy;

	[Header("Interaction")]
	//protected List<GameObject> visibleTargets;
	protected LineOfSight lineOfSight;


	InputAction moveAction;

    void Start()
    {
		moveAction = InputSystem.actions.FindAction("Move");
		lineOfSight = GetComponentInChildren<LineOfSight>();
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

		if (actions.DiscreteActions[0] == 1)
		{
			if (Attack(lineOfSight.VisibleTargets[0]))
			{
				//TODO: implement
			}
		}

		if (transform.localPosition.y < -0.5)
		{
			AddReward(-0.5f);
			EndEpisode();
		}
	}

	public override void Heuristic(in ActionBuffers actionsOut)
	{
		var continuousActions = actionsOut.ContinuousActions;
		var discreteActions = actionsOut.DiscreteActions;

		Vector2 moveValue = moveAction.ReadValue<Vector2>();

		continuousActions[0] = moveValue.x;
		continuousActions[1] = moveValue.y;

	}

	public virtual bool Attack(GameObject target)
	{
		var dirDiff = Quaternion.Angle(transform.rotation, target.transform.rotation);

		if (dirDiff > 5f)
			return false;

		var transVec = target.transform.position - transform.position;

		if (transVec.magnitude < attackRad)
			return false;

		if (Random.Range(0, 1) <= accuracy)
		{
			return false;
		}

        if (Physics.Raycast(transform.position, transVec.normalized, out RaycastHit hit))
        {
            var collider = hit.collider.gameObject;
            if (collider.layer == targetMask.value)
            {
                if(collider.TryGetComponent(out Character character))
				{
					character.TakeDamage(damage);
				}
				return true;
            }
        }
		return false;
    }

	public virtual int TakeDamage(int damage)
	{
		return damage;
	}
}
