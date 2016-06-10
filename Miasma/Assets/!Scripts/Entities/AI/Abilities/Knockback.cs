using UnityEngine;
using System.Collections;

public class Knockback : Ability
{

    public float damage;
    public float knockbackForce;
    public float attackConeAngle;
    public AudioClip attackSound;

	// Use this for initialization
    void Awake()
    {
        nav = GetComponentInChildren<NavMeshAgent>();
        ai = GetComponentInChildren<BaseAI>();
	}
	
	// Update is called once per frame
	void Update () {
        Cooldown();
	}

    // Use Ability
    public override IEnumerator UseAbility()
    {
        cooldownTimer = cooldown;
        yield return new WaitForSeconds(windUpTime);

        if (ai.targetDeltaAngle <= attackConeAngle)
        {
            if(ai.entityData[ai.targetIndex].entity.GetComponent<ImpactReceiver>())
            {
                Vector3 forceDirection = (ai.entityData[ai.targetIndex].entity.transform.position - transform.position).normalized;
                ai.entityData[ai.targetIndex].entity.GetComponent<ImpactReceiver>().AddImpact(forceDirection, knockbackForce);
            }
            ai.entityData[ai.targetIndex].entity.health -= damage;
            GetComponent<AudioSource>().PlayOneShot(attackSound);
        }
    }
}
