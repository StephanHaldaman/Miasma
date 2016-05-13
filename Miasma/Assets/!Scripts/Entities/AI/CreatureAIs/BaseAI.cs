using UnityEngine;
using System.Collections;

public class BaseAI : Entity {

    public float agro;
    public float activeSpeed;
    public float passiveSpeed;
    public PassiveEffect passiveEffect;
    public GameObject[] abilities;

    public bool inCombat;
    public Vector3 directionOfInterest;
    public float playerDetection;
    public float alertness;

    public Vector3 lastTargetLocation;
    public Entity targetEntity;
    public AwarenessData[] entities;
    
    protected AIBehaviour[] behaviours;

    protected NavMeshAgent nav;

    protected void UpdateBehavior()
    {
        if (behaviours.Length != 0)
        {
            behaviours[0].Behave();
        }
    }

    public void UpdateEntities() 
    {
        Entity[] entitiesArray = GameObject.FindObjectsOfType<Entity>();
        // Subtract one to remove own object
        entities = new AwarenessData[entitiesArray.Length - 1];

        int j = 0;
        for (int i = 0; i < entitiesArray.Length; i++)
        {
            if (Vector3.Distance(transform.position, entitiesArray[i].transform.position) > 0f)
            {
                entities[j].entity = entitiesArray[i];
                j++;
            }
        }
    }

    public void UpdateEntityAwareness()
    {
        foreach (AwarenessData entityData in entities) //LAST WORKED ON THING
        {
            RaycastHit hit;
            //Check for visibility
            if (Physics.Raycast(transform.position, (entityData.entity.transform.position + entityData.entity.headOffset), out hit)
                || Physics.Raycast(transform.position, (entityData.entity.transform.position + entityData.entity.legsOffset), out hit))
            {
                print(hit.distance + "  " + hit.collider.gameObject.name);
            }
            else
            {
                print(false);
            }
        }
    }
    
}
