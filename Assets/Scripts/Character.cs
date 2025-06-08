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
	protected LineOfSight lineOfSight;
	protected EpisodeManager episodeManager;
	private Vector3 startPos;
	private Quaternion startRot;


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
		startPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
		startRot = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
		lineOfSight.targetMask = enemyMask;
	}

	void FixedUpdate()
    {
		if (lineOfSight.VisibleTargets.Count > 0)
		{
			float dirDiff = AngleToTarget(lineOfSight.VisibleTargets[0]);
			AddReward(1f - 0.01f * dirDiff);
		} else
		{
			AddReward(-0.1f);
		}
	}

	public override void OnEpisodeBegin()
	{
		base.OnEpisodeBegin();
		healthManager.health = 20;
		transform.SetPositionAndRotation(startPos, startRot);
		Debug.Log("Starting new episode");
	}

	public override void CollectObservations(VectorSensor sensor)
	{
		bool targetVisible = lineOfSight.VisibleTargets.Count > 0;
		float dirDiff = 100f;
		if (targetVisible)
		{
			dirDiff = AngleToTarget(lineOfSight.VisibleTargets[0]);
		}

		sensor.AddObservation(healthManager.health);
		sensor.AddObservation(attackRad);
		sensor.AddObservation(targetVisible);
		sensor.AddObservation(dirDiff);
		base.CollectObservations(sensor);
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

		AddReward(-1f * 0.01f);
	}

	public override void Heuristic(in ActionBuffers actionsOut)
	{
		var continuousActions = actionsOut.ContinuousActions;
		var discreteActions = actionsOut.DiscreteActions;


		Vector2 moveValue = moveAction.ReadValue<Vector2>();
		bool attackValue = attackAction.ReadValue<bool>();

		continuousActions[0] = moveValue.x;
		continuousActions[1] = moveValue.y;
		discreteActions[0] = 1;
	}

	public static Vector3 RandomNavSphere(Vector3 origin, float distance)
	{
		Vector3 randomDirection = Random.insideUnitSphere * distance;
		randomDirection += origin;

		NavMeshHit navHit;
		NavMesh.SamplePosition(randomDirection, out navHit, distance, NavMesh.AllAreas);

		return navHit.position;
	}

	public virtual bool Attack(GameObject target, out int targetHealth)
	{
		targetHealth = -1;

		float dirDiff = AngleToTarget(target);

		if (dirDiff > 5f)
		{
			AddReward(-1f);
			return false;
		}

		var transVec = target.transform.position - transform.position;

		if (transVec.magnitude > attackRad)
		{
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
				AddReward(10f);
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

	private float AngleToTarget(GameObject target)
	{
		Vector3 toTarget = target.transform.position - transform.position;
		toTarget.y = 0;
		toTarget.Normalize();

		Vector3 forward = transform.forward;
		forward.y = 0;
		forward.Normalize();

		float dirDiff = Vector3.Angle(forward, toTarget);

		return dirDiff;
	}

	public virtual int TakeDamage(int damage)
	{
		healthManager.health -= damage;
		Debug.Log(healthManager.health);
		if (healthManager.health <= 0)
		{
			AddReward(-10f);
			episodeManager.EndAllEpisodes();
		}

		return healthManager.health;
	}
}
