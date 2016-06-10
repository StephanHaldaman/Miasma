using UnityEngine;
using System.Collections;

public class BasicAttack : Ability
{

    public float damage;
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
            ai.entityData[ai.targetIndex].entity.health -= damage;
            GetComponent<AudioSource>().PlayOneShot(attackSound);
        }
    }
}
