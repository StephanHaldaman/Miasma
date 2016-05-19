using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {

	public Text Text_Mags_Pellet;
	public Text Text_Mags_Bullet;
	public Text Text_Mags_Slug;

	// Use this for initialization
	void Start () {
		PlayerGun player = GameObject.FindGameObjectWithTag ("EntityPlayer").GetComponent<PlayerGun> ();
		Text_Mags_Pellet.text = player.MagCount_Pellet.ToString();
		Text_Mags_Bullet.text = player.MagCount_Bullet.ToString();
		Text_Mags_Slug.text = player.MagCount_Slug.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		/*PlayerGun player = GameObject.FindGameObjectWithTag ("EntityPlayer").GetComponent<PlayerGun> ();

		if (GameObject.FindGameObjectWithTag ("EntityPlayer").GetComponent<PlayerGun> ().HeldGun == null) {
			//Text_Ammo.text = player.MagazineCount.ToString();
		} else if (GameObject.FindGameObjectWithTag ("EntityPlayer").GetComponent<PlayerGun> ().HeldGun != null) {
			GunProperties gun = player.HeldGun.GetComponent<GunProperties> ();
			//Text_Ammo.text = gun.Gun_MagSize.ToString () + "/" + player.MagazineCount;
		} */
	}

	//call when something changes instead of on update.
	public void UpdateInfo() {
		PlayerGun player = GameObject.FindGameObjectWithTag ("EntityPlayer").GetComponent<PlayerGun> ();
		Text_Mags_Pellet.text = player.MagCount_Pellet.ToString();
		Text_Mags_Bullet.text = player.MagCount_Bullet.ToString();
		Text_Mags_Slug.text = player.MagCount_Slug.ToString();
	}
}
