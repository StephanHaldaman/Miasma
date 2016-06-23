using UnityEngine;
using System.Collections;

public class AICombat : AIBehaviour
{

	// Use this for initialization
    void Awake()
    {
        inCombatOnly = true;
        nav = GetComponentInChildren<NavMeshAgent>();
        ai = GetComponentInChildren<BaseAI>();
	}

    // Use Behaviour
    public override void Behave()
    {
        nav.SetDestination(ai.lastTargetLocation);

        for (int i = 0; i < ai.abilities.Length; i++)
        {
            if (ai.abilities[i].agroCost <= ai.agro)
            {
                //If in range and not cooling down then use ability
                if (Vector3.Distance(transform.position, ai.entityData[ai.targetIndex].entity.transform.position) <= ai.abilities[i].range && ai.abilities[i].cooldownTimer <= 0)
                {
                    ai.agro -= ai.abilities[i].agroCost;
                    StartCoroutine(ai.abilities[i].UseAbility());
                }
            }
        }
    }
}
