using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {

	public Text Text_Mags_Pellet;
	public Text Text_Mags_Bullet;
	public Text Text_Mags_Slug;
	public Text Text_PickUpText;
	public Image HealthIndicator;
	private float MaxHealth;
	
	// Use this for initialization
	void Start () {
		PlayerGear player = GameObject.FindGameObjectWithTag ("EntityPlayer").GetComponent<PlayerGear> ();
		Text_Mags_Pellet.text = player.MagCount_Pellet.ToString();
		Text_Mags_Bullet.text = player.MagCount_Bullet.ToString();
		Text_Mags_Slug.text = player.MagCount_Slug.ToString();

		MaxHealth = GameObject.FindGameObjectWithTag ("EntityPlayer").GetComponent<Player> ().health;
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

		UpdateHealth ();

		PlayerGun gear = GameObject.FindGameObjectWithTag ("EntityPlayer").GetComponent<PlayerGun> ();

		RaycastHit hit;
		if (Physics.Raycast (gear.myCamera.transform.position, gear.myCamera.transform.forward, out hit)) {
			if (hit.distance < 4f) {
				if (hit.collider.gameObject.CompareTag ("Item") || hit.collider.gameObject.CompareTag ("Weapon") ) {
					Text_PickUpText.text = "[E] " + hit.collider.gameObject.GetComponent<PickUp> ().ItemName;
				} else {
					Text_PickUpText.text = "";
				}
			} else {
				Text_PickUpText.text = "";
			}
		} else {
			Text_PickUpText.text = "";
		}
	}

	//call when something changes instead of on update.
	public void UpdateInfo() {
		PlayerGear player = GameObject.FindGameObjectWithTag ("EntityPlayer").GetComponent<PlayerGear> ();
		Text_Mags_Pellet.text = player.MagCount_Pellet.ToString();
		Text_Mags_Bullet.text = player.MagCount_Bullet.ToString();
		Text_Mags_Slug.text = player.MagCount_Slug.ToString();
	}

	public void UpdateHealth() {
		Player plyr = GameObject.FindGameObjectWithTag ("EntityPlayer").GetComponent<Player> ();
		float healthPercentage = plyr.health / MaxHealth;
		Image status = HealthIndicator.GetComponent<Image> ();

		if (healthPercentage >= 0.75) {
			//green
			status.color = Color.green;
		} else if (healthPercentage >= 0.5) {
			//yellow
			status.color = Color.yellow;
		} else if (healthPercentage >= 0.25) {
			//orange
			status.color = new Vector4 (1,0.5f,0,1);
		} else {
			//red
			status.color = Color.red;
		}
		
	}
}
