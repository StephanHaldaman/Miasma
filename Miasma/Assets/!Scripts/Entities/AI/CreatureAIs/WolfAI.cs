using UnityEngine;
using System.Collections;

public class WolfAI : BaseAI {

	// Use this for initialization
	void Awake () {
        agro = 20;

        UpdateEntities();
        behaviours = GetComponents<AIBehaviour>();
	}
	
	// Update is called once per frame
	void Update () {
        UpdateBehavior();
        UpdateEntityAwareness();
        UpdateTarget();
	}
}