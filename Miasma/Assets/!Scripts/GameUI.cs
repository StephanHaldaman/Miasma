using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {

	public Text Text_Ammo;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		PlayerGun player = GameObject.FindGameObjectWithTag ("EntityPlayer").GetComponent<PlayerGun> ();

		if (GameObject.FindGameObjectWithTag ("EntityPlayer").GetComponent<PlayerGun> ().HeldGun == null) {
			//Text_Ammo.text = player.MagazineCount.ToString();
		} else if (GameObject.FindGameObjectWithTag ("EntityPlayer").GetComponent<PlayerGun> ().HeldGun != null) {
			GunProperties gun = player.HeldGun.GetComponent<GunProperties> ();
			Text_Ammo.text = gun.Gun_MagSize.ToString () + "/" + player.MagazineCount;
		} 
	}
}
