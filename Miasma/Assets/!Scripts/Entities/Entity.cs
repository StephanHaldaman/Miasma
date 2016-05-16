using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {

    public float health;
    public float visibility;
    public Vector3 headOffset;
    public Vector3 legsOffset;
    public ParticleSystem particles_BloodSpray;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Bullet"))
        {
            health -= other.collider.GetComponent<DestroyOnCollide>().Damage;
            ParticleSystem blood = Instantiate(particles_BloodSpray, other.contacts[0].point, transform.rotation) as ParticleSystem;
            blood.transform.parent = other.contacts[0].thisCollider.transform;
        }

        if (health <= 0)
        {
            if (GetComponentInChildren<NavMeshAgent>())
            {
                GetComponentInChildren<NavMeshAgent>().enabled = false;
                GetComponent<Entity>().enabled = false;
                GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }
}
