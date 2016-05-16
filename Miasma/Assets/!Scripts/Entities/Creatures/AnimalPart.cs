using UnityEngine;
using System.Collections;
using DG.Tweening;

public class AnimalPart : MonoBehaviour {

	public GameObject[] myParts;
	public ParticleSystem particles_BloodSpray;
	public float HealthMax = 10;
	private float Health;

	// Use this for initialization
	void Start () {
		Health = HealthMax;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	
}
