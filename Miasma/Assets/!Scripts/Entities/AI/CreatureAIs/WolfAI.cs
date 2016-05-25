using UnityEngine;
using System.Collections;

public class WolfAI : BaseAI {


	// Use this for initialization
	void Awake () {
        UpdateEntities();
        behaviours = GetComponents<AIBehaviour>();
        abilities = GetComponents<Ability>();
	}
	
	// Update is called once per frame
	void Update () {
        UpdateBehavior();
        UpdateEntityAwareness();
        UpdateTarget();
        UpdateAgro();
	}
}