using UnityEngine;
using System.Collections;

public class WolfAI : BaseAI {

	// Use this for initialization
	void Awake () {
        agro = 20;

        UpdateEntities();
        behaviours = GetComponents<AIBehaviour>();
	}
	
	// Update is called once per frame
	void Update () {
        UpdateBehavior();
        UpdateEntityAwareness();
        UpdateTarget();
	}

    void OnCollisionEnter(Collision other)
    {
        Debug.Log("Other: " + other.collider.name);
        if (other.collider.CompareTag("Bullet"))
        {

            foreach (ContactPoint c in other.contacts)
            {
                if (c.thisCollider.name.Contains("Part"))
                {
                    health -= other.collider.GetComponent<DestroyOnCollide>().Damage;
                    ParticleSystem blood = Instantiate(particles_BloodSpray, other.contacts[0].point, transform.rotation) as ParticleSystem;
                }
            }
        }
    }
}