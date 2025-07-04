using UnityEngine;
using System.Collections.Generic;

public class LineOfSight : MonoBehaviour
{
    public float viewRadius;
    [Range(0,360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public List<GameObject> VisibleTargets = new List<GameObject>();

	private void Start()
	{
        InvokeRepeating("FindVisibleTargets", .2f, .2f);
	}

    void FindVisibleTargets()
    {
        VisibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        foreach (var target in targetsInViewRadius)
        {
			Vector3 dirToTarget = (target.transform.position - transform.position);

			if (Vector3.Angle(transform.forward, dirToTarget.normalized) < viewAngle / 2)
            {
                if (!Physics.Raycast(transform.position, dirToTarget, dirToTarget.magnitude, obstacleMask))
                {
                    VisibleTargets.Add(target.gameObject);
                }
            }
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
