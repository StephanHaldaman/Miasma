using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Explosion : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.DOScale (3, 0.1f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
