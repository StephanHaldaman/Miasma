using UnityEngine;
using System.Collections;

public class GunProperties : MonoBehaviour {

	public GameObject Bullet;
	public GameObject SpentShell;
	public GameObject MyMagazine;
	public GameObject MyMagType;
	public Transform Pos_Barrel;
	public Transform Pos_Ejecter;
	public Transform Pos_Mag;
	public float Bullet_Velocity = 250;
	public int Bullet_PerShot = 1;
	public int Gun_FireMode = 0; //0: Single Shot; 1: Automatic; 2: Burst
	public float Gun_FireRate = 0.1f;
	public int Gun_MagSizeTotal = 30;
	public int Gun_MagSize = 30;
	public float Bullet_xSpin = 0.5f;
	public float Bullet_ySpin = 0.5f;
	public float Gun_xRecoil = 1;
	public float Gun_yRecoil = 1;
	public AudioClip sfx_Shot;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
