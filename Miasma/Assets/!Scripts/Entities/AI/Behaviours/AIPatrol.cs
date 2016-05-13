using UnityEngine;
using System.Collections;

public class AIPatrol : AIBehaviour
{

    public Transform[] patrolPoints;
    public int patrolIndex = 0;

	// Use this for initialization
	void Awake () {
        nav = GetComponent<NavMeshAgent>();
        ai = GetComponent<BaseAI>();
	}
	
	// Use Behaviour
    public override void Behave()
    {
        // Set speed to passive
        nav.speed = ai.passiveSpeed;

        if (nav.remainingDistance <= nav.stoppingDistance)
        {
            // Check if there are no more patrol points
            if (patrolIndex != (patrolPoints.Length - 1))
            {
                nav.SetDestination(patrolPoints[patrolIndex].position);
                patrolIndex++;
            }
            else
            {
                nav.SetDestination(patrolPoints[patrolIndex].position);
                patrolIndex = 0;
            }
        }
	}
}
