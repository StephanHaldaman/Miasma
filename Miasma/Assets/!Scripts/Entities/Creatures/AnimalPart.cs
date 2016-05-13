using UnityEngine;
using System.Collections;
using DG.Tweening;

public class AnimalPart : MonoBehaviour {

	public GameObject[] myParts;
	public ParticleSystem particles_BloodSpray;
	public float HealthMax = 10;
	private float Health;

	// Use this for initialization
	void Start () {
		Health = HealthMax;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter (Collision other) {
		Debug.Log ("Other: " + other.collider.name);
		if(other.collider.CompareTag("Bullet")){

			foreach (ContactPoint c in other.contacts)
			{
				if(c.thisCollider.name.Contains("Part")){
					c.thisCollider.GetComponent<AnimalPart>().Health -= other.collider.GetComponent<DestroyOnCollide>().Damage;

					if(c.thisCollider.GetComponent<AnimalPart>().Health > 0){
						c.thisCollider.transform.DOComplete();
						c.thisCollider.transform.DOShakePosition(0.25f,0.1f,30,90);
					} else { //limb falls off if health < 0
						c.thisCollider.transform.parent = null;
						c.thisCollider.gameObject.AddComponent<Rigidbody>();
						c.thisCollider.gameObject.GetComponent<Rigidbody>().AddForce(other.collider.GetComponent<Rigidbody>().velocity, ForceMode.Force);
					}

				} else if (c.thisCollider.name.Contains("Core")){
					//c.thisCollider.GetComponent<AnimalPart>().Health -= other.collider.GetComponent<DestroyOnCollide>().Damage;
					ParticleSystem blood = Instantiate(particles_BloodSpray, other.contacts[0].point, transform.rotation) as ParticleSystem;
					blood.transform.parent = c.thisCollider.transform;
				} 
				Debug.Log(c.thisCollider.name);
			}
		}
	}
}
