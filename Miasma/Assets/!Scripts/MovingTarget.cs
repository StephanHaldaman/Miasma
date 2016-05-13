using UnityEngine;
using System.Collections;

public class MovingTarget : MonoBehaviour {

	public float MovementRadius;
	public int Health;
	private Vector3 StartingPos;
	private bool MovingLeft;
	public float Speed = 0.05f;
	public GameObject BloodParticles;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.x+(StartingPos.x) > MovementRadius+(StartingPos.x)) {
			MovingLeft = false;
		} else if (transform.position.x-(StartingPos.x) < -MovementRadius+(StartingPos.x)) {
			MovingLeft = true;
		}

		if (Health > 0) {
			if (MovingLeft) {
				transform.position += new Vector3 (Speed, 0, 0);
			} else {
				transform.position -= new Vector3 (Speed, 0, 0);
			}
		} else {
			this.GetComponent<Rigidbody>().freezeRotation = false;
		}
	}
	
	void OnCollisionEnter (Collision other) {
		if(other.collider.CompareTag("Bullet")){
			GameObject blood = Instantiate(BloodParticles, other.contacts[0].point, transform.rotation) as GameObject;
			blood.transform.parent = gameObject.transform;
			Debug.Log("Damaged!");
			Health -= 1;
		}
	}
}
