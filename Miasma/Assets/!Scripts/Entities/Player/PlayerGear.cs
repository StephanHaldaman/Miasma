using UnityEngine;
using System.Collections;

public class PlayerGear : MonoBehaviour {

	private Camera myCamera;

	public int MagCount_Pellet;
	public int MagCount_Bullet;
	public int MagCount_Slug;
	public int TrapCount;

	// Use this for initialization
	void Start () {
		myCamera = GetComponent<PlayerGun> ().myCamera;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.T)) {
			//GameObject grenade = Instantiate(Grenade_Frag, transform.position, transform.rotation) as GameObject;
			//grenade.GetComponent<Rigidbody>().AddForce(myCamera.transform.forward * 20, ForceMode.Impulse);
		}

		WeaponGet ();
	}

	void ThrowGrenade () {

	}

	void WeaponGet () {
		GameObject HeldGun = GetComponent<PlayerGun> ().HeldGun;



		if (Input.GetKeyDown (KeyCode.E)) {
			RaycastHit hit;
			if (Physics.Raycast (myCamera.transform.position, myCamera.transform.forward, out hit)) {
				if (hit.distance < 4f) {
					if (hit.collider.gameObject.CompareTag ("Weapon")) {
						if (HeldGun == null) {
							GetComponent<PlayerGun> ().HeldGun = hit.collider.gameObject;
							GetComponent<PlayerGun> ().HeldGun.GetComponent<Collider>().enabled = false;
							Destroy (GetComponent<PlayerGun> ().HeldGun.GetComponent<Rigidbody> ());
							GetComponent<PlayerGun> ().HeldGun.transform.parent = GetComponent<PlayerGun>().GunArm.transform;
							GetComponent<PlayerGun> ().HeldGun.transform.localPosition = Vector3.zero;
							GetComponent<PlayerGun> ().HeldGun.transform.rotation = GetComponent<PlayerGun>().GunArm.transform.rotation;
						}
					} else if (hit.collider.gameObject.CompareTag ("Item")) {
						hit.collider.gameObject.GetComponent<PickUp>().OnPickUp();
					}
				}
			}
		} else if (Input.GetKeyDown (KeyCode.G)) {
			if(HeldGun != null){
				GetComponent<PlayerGun> ().HeldGun.GetComponent<Collider>().enabled = true;
				GetComponent<PlayerGun> ().HeldGun.AddComponent<Rigidbody>();
				GetComponent<PlayerGun> ().HeldGun.transform.parent = null;
				GetComponent<PlayerGun> ().HeldGun = null;
			}
		}


	}
}
