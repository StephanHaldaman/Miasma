using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DestroyOnCollide : MonoBehaviour {

	public GameObject CreateOnDestroy;
	private float CheckTime = 0.25f;
	private bool Dead;
	private AudioSource sfx;
	public AudioClip sfx_Flying;
	public AudioClip sfx_Ricochet;
	public float Damage;

	// Use this for initialization
	void Start () {
		sfx = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!Dead) {
			if (CheckTime <= 0) { //if you dont do the timer, the bullet sometimes just destroys itself immediately.
				if (GetComponent<Rigidbody> ().velocity.magnitude <= 1) {
					BeginKillBullet ();
				}
			} else {
				CheckTime -= Time.deltaTime;
			}
		}
	}

	void OnCollisionEnter(Collision other){
		if (other.collider.GetComponent<MeshRenderer> () != null) {
			if (other.collider.GetComponent<MeshRenderer> ().material.name.Contains ("Metal") || other.collider.GetComponent<MeshRenderer> ().material.name.Contains ("Prototype")) {
				//do nothing. bullet should bounce off metal
				sfx.pitch = Random.Range(0.75f,1.25f);
				sfx.PlayOneShot(sfx_Ricochet);
			} else {
				Instantiate (CreateOnDestroy, transform.position, transform.rotation);
				BeginKillBullet();
			}
		} else {
			//its probably terrain if it has no mesh renderer
			BeginKillBullet();
		}
	}

	void BeginKillBullet(){
		Destroy (GetComponent<Rigidbody> ());
		Destroy (GetComponent<Collider> ());
		GetComponent<Light>().DOIntensity(0,0.25f);
		Invoke ("KillBullet", 0.25f);
		Dead = true;
	}

	void KillBullet(){
		Destroy (gameObject);
	}
}
