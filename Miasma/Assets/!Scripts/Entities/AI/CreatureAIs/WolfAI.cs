using UnityEngine;
using System.Collections;

public class WolfAI : BaseAI {

    public float agroGain;

	// Use this for initialization
	void Awake () {
        agro = 20;

        UpdateEntities();
        behaviours = GetComponents<AIBehaviour>();
        //abilities = GetComponents<Ability>();
	}
	
	// Update is called once per frame
	void Update () {
        UpdateBehavior();
        UpdateEntityAwareness();
        UpdateTarget();

        if (entityData[targetIndex].isAware)
        {
            agro += agroGain * Time.deltaTime;
        }
	}
}