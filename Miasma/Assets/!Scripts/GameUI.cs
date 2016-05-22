using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

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
		HealthBeat ();
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

	private void HealthBeat() {
		Player plyr = GameObject.FindGameObjectWithTag ("EntityPlayer").GetComponent<Player> ();
		float healthPercentage = plyr.health / MaxHealth;
		Image status = HealthIndicator.GetComponent<Image> ();
		status.transform.localScale = new Vector3 (1, 1, 1);
		CancelInvoke ();
		status.transform.DOComplete ();

		if (healthPercentage >= 0.75) {
			status.transform.DOScale(0.75f, 3);
			Invoke("HealthBeat", 3);
		} else if (healthPercentage >= 0.5) {
			status.transform.DOScale(0.75f, 1);
			Invoke("HealthBeat", 1);
		} else if (healthPercentage >= 0.25) {
			status.transform.DOScale(0.75f, 0.35f);
			Invoke("HealthBeat", 0.35f);
		} else {
			status.transform.DOScale(0.75f, 0.15f);
			Invoke("HealthBeat", 0.15f);
		}
	}
}
