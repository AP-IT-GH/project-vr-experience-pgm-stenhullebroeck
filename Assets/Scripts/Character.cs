using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Unity.MLAgents.Sensors;
using UnityEngine.AI;

public class Character : Agent
{
	protected HealthManager healthManager;
	[SerializeField]
	protected int damage;
	[SerializeField]
    protected float movementSpd = 5;
	[SerializeField]
	protected float rotationSpd = 90f;

	[Header("Attack")]
	public LayerMask obstacleMask;
	public LayerMask friendlyMask;
	public LayerMask enemyMask;
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
	protected EpisodeManager episodeManager;
	//private NavMeshAgent navAgent;


	InputAction moveAction;
	InputAction attackAction;

	protected float attackCooldown = 0f;

	void Start()
    {
		moveAction = InputSystem.actions.FindAction("Move");
		attackAction = InputSystem.actions.FindAction("Attack");
		lineOfSight = GetComponentInChildren<LineOfSight>();
		episodeManager = GetComponentInParent<EpisodeManager>();
		healthManager = GetComponentInChildren<HealthManager>();
		//navAgent = GetComponent<NavMeshAgent>();
	}

	void FixedUpdate()
    {
	}

	public override void OnEpisodeBegin()
	{
		base.OnEpisodeBegin();
		healthManager.health = 5;
		Debug.Log("Starting new episode");
	}

	public override void CollectObservations(VectorSensor sensor)
	{
		bool targetVisible = lineOfSight.VisibleTargets.Count > 0;

		base.CollectObservations(sensor);
		sensor.AddObservation(healthManager.health);
		sensor.AddObservation(attackRad);
		sensor.AddObservation(targetVisible);
	}

	public override void OnActionReceived(ActionBuffers actions)
	{
		if (healthManager.health <= 0)
			return;

		attackCooldown = Mathf.Max(0f, attackCooldown - Time.deltaTime);

		Vector3 controller = Vector3.zero;

		controller.x = actions.ContinuousActions[0];
		controller.z = actions.ContinuousActions[1];


		transform.Translate(Vector3.forward * controller.z * this.movementSpd * Time.deltaTime);
		transform.Rotate(Vector3.up, controller.x * rotationSpd * Time.deltaTime, Space.Self);

		//if (lineOfSight.VisibleTargets.Count > 0)
		//{
		//	if (navAgent.enabled) navAgent.isStopped = true;
		//}
		//else
		//{
		//	if (!navAgent.enabled) navAgent.enabled = true;

		//	if (!navAgent.hasPath || navAgent.remainingDistance < 0.5f)
		//	{
		//		Vector3 randomDestination = RandomNavSphere(transform.position, 10f);
		//		navAgent.SetDestination(randomDestination);
		//	}

		//	AddReward(-0.001f);
		//}

		AddReward(-1f * 0.001f);
	}

	public override void Heuristic(in ActionBuffers actionsOut)
	{
		var continuousActions = actionsOut.ContinuousActions;
		var discreteActions = actionsOut.DiscreteActions;


		Vector2 moveValue = moveAction.ReadValue<Vector2>();
		bool attackValue = attackAction.ReadValue<bool>();
		GameObject target = lineOfSight.VisibleTargets[0];

		continuousActions[0] = moveValue.x;
		continuousActions[1] = moveValue.y;
		discreteActions[0] = 1;
	}

	//public static Vector3 RandomNavSphere(Vector3 origin, float distance)
	//{
	//	Vector3 randomDirection = Random.insideUnitSphere * distance;
	//	randomDirection += origin;

	//	NavMeshHit navHit;
	//	NavMesh.SamplePosition(randomDirection, out navHit, distance, NavMesh.AllAreas);

	//	return navHit.position;
	//}

	public virtual bool Attack(GameObject target, out int targetHealth)
	{
		targetHealth = -1;

		//var dirDiff = Quaternion.Angle(transform.rotation, target.transform.rotation);

		Vector3 toTarget = target.transform.position - transform.position;
		toTarget.y = 0;
		toTarget.Normalize();

		Vector3 forward = transform.forward;
		forward.y = 0;
		forward.Normalize();

		float dirDiff = Vector3.Angle(forward, toTarget);

		if (dirDiff > 5f)
		{
			AddReward(-0.01f);
			return false;
		}

		var transVec = target.transform.position - transform.position;

		if (transVec.magnitude > attackRad)
		{
			AddReward(-0.01f);
			return false;
		}

		if (Random.Range(0f, 1f) > accuracy)
		{
			return false;
		}

        if (Physics.Raycast(transform.position + transVec.normalized * 0.5f, transVec.normalized, out RaycastHit hit, attackRad, enemyMask))
        {
            var collider = hit.collider.gameObject;

			if (((1 << collider.layer) & enemyMask.value) != 0)
			{
				AddReward(1f);
                if(collider.TryGetComponent(out Character character))
				{
					targetHealth = character.TakeDamage(damage);
					Debug.Log(targetHealth);
					return true;
				}
            }
        }
		return false;
    }

	public virtual int TakeDamage(int damage)
	{
		healthManager.health -= damage;
		Debug.Log(healthManager.health);

		return healthManager.health;
	}
}
