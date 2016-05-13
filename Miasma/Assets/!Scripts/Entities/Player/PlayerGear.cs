using UnityEngine;
using System.Collections;

public class PlayerGear : MonoBehaviour {

	private Camera MyCamera;
	public GameObject Grenade_Frag;
	public GameObject Grenade_Perc;

	// Use this for initialization
	void Start () {
		MyCamera = GetComponent<PlayerGun> ().myCamera;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.T)) {
			GameObject grenade = Instantiate(Grenade_Frag, transform.position, transform.rotation) as GameObject;
			grenade.GetComponent<Rigidbody>().AddForce(MyCamera.transform.forward * 20, ForceMode.Impulse);
		}
	}

	void ThrowGrenade () {

	}
}
