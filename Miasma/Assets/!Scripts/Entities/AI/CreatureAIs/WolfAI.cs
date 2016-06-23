using UnityEngine;
using System.Collections;

public class WolfAI : BaseAI {


	// Use this for initialization
    void Awake()
    {
        UpdateEntities();
        behaviours = GetComponents<AIBehaviour>();
        abilities = GetComponents<Ability>();
        nav = GetComponent<NavMeshAgent>();

        // Set speed to passive by default
        nav.speed = passiveSpeed;
	}
	
	// Update is called once per frame
	void Update () {
        UpdateBehavior();
        UpdateEntityAwareness();
        UpdateTarget();
        UpdateAgro();
	}
}