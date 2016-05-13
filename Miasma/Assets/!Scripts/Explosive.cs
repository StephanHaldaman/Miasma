using UnityEngine;
using System.Collections;

public class Explosive : MonoBehaviour {

	public GameObject Shrapnel;
	public float ShrapnelAmount;
	public float ShrapnelForce;
	public GameObject ExplosionSound;
	public GameObject particles_Explosion;
	public bool ExplodeOnImpact;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.T)) {
			//Explode();
		}
	}

	void OnCollisionEnter (Collision other){
		Explode ();
	}

	void Explode () {

		for(int i = 0; i < ShrapnelAmount; i++){

			GameObject bullet = Instantiate(Shrapnel, transform.position, transform.rotation) as GameObject;
			//bullet.transform.Rotate (new Vector3 (Random.Range (-180, 180), Random.Range (-180, 180), Random.Range (-180, 180)));
			bullet.transform.Rotate (new Vector3 (0, Random.Range (0, 360), Random.Range (0,360)));
			//bullet.GetComponent<Rigidbody>().AddForce((bullet.transform.forward) * Random.Range(ShrapnelForce/1.5f, ShrapnelForce*1.5f), ForceMode.Impulse);
			bullet.GetComponent<Rigidbody>().AddForce((bullet.transform.forward) * ShrapnelForce, ForceMode.Impulse);
		}
		Destroy (gameObject);
		Instantiate(ExplosionSound, transform.position, transform.rotation);
		Instantiate(particles_Explosion, transform.position, transform.rotation);
	}
}
