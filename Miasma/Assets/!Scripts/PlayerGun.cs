using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using DG.Tweening;

public class PlayerGun : MonoBehaviour {
	
	public Camera myCamera;
	public GameObject HeldGun;
	public GameObject GunArm;
	private Transform Pos_Current; //for position only, not rotation	
	public Transform Pos_GunDown;
	public Transform Pos_GunUp;
	public Transform Pos_GunReloading;
	public Transform Pos_GunAim;
	private bool AimMode;



	private bool IsShooting;
	private bool WasShooting;

	public GameObject Particles_GunSmoke;

	private bool ShotTrigger;
	private float NextFireTime;

	//public int MagCount_Pellet;
	//public int MagCount_Bullet;
	//public int MagCount_Slug;

	//move to gun
	//public Transform myBarrel;

	private bool IsReloading;
	private bool CanFire;
	private bool IsFiring;
	
	private Vector3 targetAngle;
	public Vector3 targetAngleOffset; //for gun recoil
	private Vector3 currentAngle;
	public float rotationSpeed = 1f;

	private AudioSource sfx;

	// Use this for initialization
	void Start () {
		sfx = GetComponent<AudioSource> ();
		Pos_Current = Pos_GunUp;
		currentAngle = transform.eulerAngles;

		if (HeldGun != null) {
			Destroy (HeldGun.GetComponent<Rigidbody> ());
		}
	}
	
	// Update is called once per frame
	void Update () {
		Cursor.lockState = CursorLockMode.Locked;
		if (Input.GetKeyDown (KeyCode.P)) {
			Application.LoadLevel(Application.loadedLevelName);
		}

		if (Input.GetKeyDown (KeyCode.R)) {
			Reload();
		}

		//WeaponGet ();

		if (Input.GetButton ("Fire2")) {
			if(!AimMode){
				SetAimMode();
			}
		} else {
			if(AimMode){
				CancelAimMode();
			}
		}

		if (HeldGun != null) {
			Shoot ();
		}
	}

	void FixedUpdate () {
		if (HeldGun != null) {
			GetGunAngle ();
			RotateGun ();
		}

	}

	void SetAimMode(){
		myCamera.GetComponent<Camera> ().DOComplete();
		myCamera.GetComponent<Camera> ().DOFieldOfView (20, 0.25f);
		Pos_Current = Pos_GunAim;
		GunArm.transform.DOComplete ();
		GunArm.transform.DOLocalMove(Pos_Current.transform.localPosition, 0.25f);
		AimMode = true;
	}

	void CancelAimMode(){
		myCamera.GetComponent<Camera> ().DOComplete();
		myCamera.GetComponent<Camera> ().DOFieldOfView (60, 0.25f);
		Pos_Current = Pos_GunUp;
		GunArm.transform.DOComplete ();
		GunArm.transform.DOLocalMove(Pos_Current.transform.localPosition, 0.25f);
		AimMode = false;
	}

	/*void WeaponGet () {
		if (Input.GetKeyDown (KeyCode.E)) {
			RaycastHit hit;
			if (HeldGun == null) {
				if (Physics.Raycast (myCamera.transform.position, myCamera.transform.forward, out hit)) {
					Debug.Log (hit.collider.name);
					if (hit.distance < 4f) {
						if (hit.collider.gameObject.CompareTag ("Weapon")) {
							HeldGun = hit.collider.gameObject;
							HeldGun.GetComponent<Collider>().enabled = false;
							Destroy (HeldGun.GetComponent<Rigidbody> ());
							HeldGun.transform.parent = GunArm.transform;
							HeldGun.transform.localPosition = Vector3.zero;
							HeldGun.transform.rotation = GunArm.transform.rotation;
						}
					}
				}
			}
		} else if (Input.GetKeyDown (KeyCode.G)) {
			if(HeldGun != null){
				HeldGun.GetComponent<Collider>().enabled = true;
				HeldGun.AddComponent<Rigidbody>();
				HeldGun.transform.parent = null;
				HeldGun = null;
			}
		}
	}*/

	void Shoot () {
		GunProperties gun = HeldGun.GetComponent<GunProperties>();

		if (Input.GetButton("Fire1")) {
			if(CanFire){
				if(gun.Gun_MagSize > 0 && !ShotTrigger){
					if(Time.time >= NextFireTime){
						for(int i = 0; i < gun.Bullet_PerShot; i++){
							LaunchBullet();
						}

						if(gun.Gun_FireMode == 0){
							ShotTrigger = true;
						}
						gun.Gun_MagSize -= 1;
						gun.ShotsBeforeBreak -= 1;
						sfx.pitch = Random.Range(0.85f,1.15f);
						sfx.PlayOneShot(gun.sfx_Shot);
						GameObject shell = Instantiate(gun.SpentShell, gun.Pos_Ejecter.position, gun.Pos_Ejecter.rotation) as GameObject;
						shell.transform.Rotate (new Vector3 (Random.Range (-30, 30), Random.Range (-30, 30), 0));
						shell.GetComponent<Rigidbody>().AddForce((transform.up+transform.right), ForceMode.Impulse);
						targetAngleOffset += new Vector3(-gun.Gun_yRecoil, Random.Range(-gun.Gun_xRecoil, gun.Gun_xRecoil), 0);
						CreateGunSmoke();

						if(gun.ShotsBeforeBreak <= 0){

							gun.BreakApart();
							GetComponent<PlayerGun> ().HeldGun.GetComponent<Collider>().enabled = true;
							GetComponent<PlayerGun> ().HeldGun.AddComponent<Rigidbody>();
							GetComponent<PlayerGun> ().HeldGun.transform.parent = null;
							GetComponent<PlayerGun> ().HeldGun = null;
						}
					}


				}
			}
		}

		//release trigger
		if (Input.GetButtonUp ("Fire1")) {
			ShotTrigger = false;
		}
	}

	void LaunchBullet(){
		GunProperties gun = HeldGun.GetComponent<GunProperties>();

		GameObject bullet = Instantiate(gun.Bullet, gun.Pos_Barrel.position, HeldGun.transform.rotation) as GameObject;
		bullet.transform.Rotate (new Vector3 (Random.Range (-gun.Bullet_xSpin, gun.Bullet_xSpin), Random.Range (-gun.Bullet_ySpin, gun.Bullet_ySpin), 0));
		bullet.GetComponent<Rigidbody> ().velocity = GetComponent<CharacterController> ().velocity;
		bullet.GetComponent<Rigidbody>().AddForce((bullet.transform.forward) * gun.Bullet_Velocity, ForceMode.Impulse);
		bullet.GetComponent<Rigidbody>().AddForce((bullet.transform.up) * (gun.Bullet_Velocity/75), ForceMode.Impulse); //hop-up
		NextFireTime = Time.time + gun.Gun_FireRate;
		
		GunArm.transform.DOComplete();
		
		GunArm.transform.position -= (HeldGun.transform.forward/4);
		GunArm.transform.DOLocalMove(Pos_Current.transform.localPosition, 0.25f);

		//GunArm.transform.Rotate(new Vector3(Random.Range(-10f,10f),Random.Range(-10f,10f),0));
		
		//GunArm.transform.localPosition -= (HeldGun.transform.forward/4);
		//GunArm.transform.DOLocalMove(Pos_GunUp.transform.localPosition, 0.25f);
		
		//GunArm.transform.localRotation = Quaternion.AngleAxis(Random.value*10,Vector3.forward);


	}

	void Reload(){
		GunProperties gunProps = HeldGun.GetComponent<GunProperties> ();
		PlayerGear gear = GetComponent<PlayerGear> ();

		if(gear.MagCount_Pellet > 0 && gunProps.MyMagType.name.Contains("Pellet")
		   || gear.MagCount_Bullet > 0 && gunProps.MyMagType.name.Contains("Bullet")
		   || gear.MagCount_Slug > 0 && gunProps.MyMagType.name.Contains("Slug")){
			IsReloading = true;
			Invoke ("ReloadEnd", 1f);

			//magazine animation
			gunProps.MyMagazine.transform.parent = null;
			gunProps.MyMagazine.AddComponent<Rigidbody>();
			GameObject mag = Instantiate(gunProps.MyMagType, transform.position, transform.rotation) as GameObject;
			gunProps.MyMagazine = mag;
			mag.transform.parent = gunProps.Pos_Mag;
			mag.transform.DOLocalMove(Vector3.zero, 0.9f);
			mag.transform.DOLocalRotate(Vector3.zero, 0.5f);
		}
	}

	void ReloadEnd(){
		GunProperties gunProps = HeldGun.GetComponent<GunProperties> ();
		PlayerGear gear = GetComponent<PlayerGear> ();

		if (gunProps.MyMagType.name.Contains ("Pellet")) {
			gear.MagCount_Pellet -= 1;
		} else if (gunProps.MyMagType.name.Contains ("Bullet")) {
			gear.MagCount_Bullet -= 1;
		} else if (gunProps.MyMagType.name.Contains ("Slug")) {
			gear.MagCount_Slug -= 1;
		}
		HeldGun.GetComponent<GunProperties>().Gun_MagSize = HeldGun.GetComponent<GunProperties>().Gun_MagSizeTotal;
		IsReloading = false;
		GameObject.FindGameObjectWithTag ("Canvas").GetComponent<GameUI> ().UpdateInfo ();
	}

	void CreateGunSmoke(){
		GunProperties gun = HeldGun.GetComponent<GunProperties>();
		GameObject smoke = Instantiate (Particles_GunSmoke, gun.Pos_Barrel.position, gun.Pos_Barrel.rotation) as GameObject;
		smoke.transform.parent = gameObject.transform;

	}

	void GetGunAngle(){
		Debug.DrawRay (HeldGun.GetComponent<GunProperties>().Pos_Barrel.position, HeldGun.transform.forward, Color.blue);
		
		RaycastHit hit; 
		Physics.Raycast (myCamera.transform.position, myCamera.transform.forward, out hit);
		
		if (!IsReloading) {
			if (hit.collider != null) {
				if (hit.distance < 0.1f) {
					targetAngle = Pos_GunDown.eulerAngles;
					CanFire = false;
				} else {
					CanFire = true;
					HeldGun.GetComponent<GunProperties> ().Pos_Barrel.LookAt (hit.point);
					targetAngle = HeldGun.GetComponent<GunProperties> ().Pos_Barrel.eulerAngles + targetAngleOffset;
				}
			} else {
				CanFire = true;
				targetAngle = myCamera.transform.eulerAngles + targetAngleOffset;
			}
		} else {
			targetAngle = Pos_GunReloading.eulerAngles + targetAngleOffset;
		}

		if (targetAngleOffset.magnitude != 0f) {
			targetAngleOffset = Vector3.Lerp(targetAngleOffset, Vector3.zero, HeldGun.GetComponent<GunProperties>().Gun_FireRate*2);

		}
	}
	
	void RotateGun () {
		currentAngle = new Vector3(
			Mathf.LerpAngle(currentAngle.x, targetAngle.x, Time.deltaTime*rotationSpeed),
			Mathf.LerpAngle(currentAngle.y, targetAngle.y, Time.deltaTime*rotationSpeed),
			Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime*rotationSpeed));
		
		
		GunArm.transform.eulerAngles = currentAngle;
	}
}
