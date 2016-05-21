using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour {

	public string ItemName = "Unnamed";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnPickUp(){
		if (ItemName == "Pellet Mag") {
			GameObject.FindGameObjectWithTag ("EntityPlayer").GetComponent<PlayerGear> ().MagCount_Pellet += 1;
		} else if (ItemName == "Bullet Mag") {
			GameObject.FindGameObjectWithTag ("EntityPlayer").GetComponent<PlayerGear> ().MagCount_Bullet += 1;
		} else if (ItemName == "Slug Mag") {
			GameObject.FindGameObjectWithTag ("EntityPlayer").GetComponent<PlayerGear> ().MagCount_Slug += 1;
		} else if (ItemName == "Explosive") {
			GameObject.FindGameObjectWithTag ("EntityPlayer").GetComponent<PlayerGear> ().TrapCount += 1;
		}
		GameObject.FindGameObjectWithTag ("Canvas").GetComponent<GameUI> ().UpdateInfo ();
		Destroy (this.gameObject);
	}
}
