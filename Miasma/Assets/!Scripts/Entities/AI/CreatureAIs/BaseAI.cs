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
    public int targetIndex = int.MaxValue;
    public AwarenessData[] entityData;
    
    protected AIBehaviour[] behaviours;

    protected NavMeshAgent nav;

    private float _deltaAwareness;

    protected void UpdateBehavior()
    {
        //If the AI has a target then it is in combat
        if (targetIndex != int.MaxValue)
        {
            inCombat = true;
        }
        else
        {
            inCombat = false;
        }

        //WORK IN PROGRESS
        if (inCombat)
        {
            behaviours[1].Behave();
        }
        else
        {
            behaviours[0].Behave();
        }
    }

    public void UpdateEntities() 
    {
        Entity[] entitiesArray = GameObject.FindObjectsOfType<Entity>();
        // Subtract one to remove own object
        entityData = new AwarenessData[entitiesArray.Length - 1];

        int j = 0;
        for (int i = 0; i < entitiesArray.Length; i++)
        {
            if (Vector3.Distance(transform.position, entitiesArray[i].transform.position) > 0f)
            {
                entityData[j].entity = entitiesArray[i];
                j++;
            }
        }
    }

    public void UpdateEntityAwareness()
    {

        for (int i = 0; i < entityData.Length; i++) //LAST WORKED ON THING
        {

            RaycastHit hit;
            //Check for visibility
            if (Physics.Raycast(transform.position, (entityData[i].entity.transform.position - transform.position) + entityData[i].entity.headOffset, out hit)
                || Physics.Raycast(transform.position, (entityData[i].entity.transform.position - transform.position) + entityData[i].entity.legsOffset, out hit))
            {
                //NEEDS BETTER DETECTION METHOD
                if (hit.collider.GetComponentInParent<Entity>() == entityData[i].entity || hit.collider.GetComponent<Entity>() == entityData[i].entity)
                {
                    //Determine how much awareness is gained per second based on distance
                    _deltaAwareness = (alertness / Vector3.Distance(transform.position, entityData[i].entity.transform.position)) * Time.deltaTime;
                    //Find delta-angle between the directional angle and the entities current heading
                    float _deltaAngle = Mathf.DeltaAngle(Quaternion.LookRotation((entityData[i].entity.transform.position - transform.position)).eulerAngles.y, transform.eulerAngles.y);
                    //Multiply awareness based on view of target
                    _deltaAwareness *= Mathf.Clamp(Mathf.Cos(Mathf.Abs(_deltaAngle) * Mathf.Deg2Rad), 0, 1);
                    //Don't let awareness past 100
                    entityData[i].awareness = Mathf.MoveTowards(entityData[i].awareness, 100, _deltaAwareness * entityData[i].entity.visibility);

                    //Set Awareness
                    if (entityData[i].awareness == 100)
                    {
                        entityData[i].isAware = true;
                    }
                    else
                    {
                        entityData[i].isAware = false;
                    }

                    //TESTING
                    print(entityData[i].awareness + "     " + _deltaAwareness / Time.deltaTime);
                }
                else 
                {
                    //Flatline change in awareness
                    _deltaAwareness = 0;
                }
            }

            //Deplete Awareness
            if (_deltaAwareness <= 0)
            {
                entityData[i].awareness = Mathf.MoveTowards(entityData[i].awareness, 0, 1 * Time.deltaTime);
            }
        }
    }

    public void UpdateTarget() 
    {
        if (targetIndex == int.MaxValue)
        {
            float _distance = float.MaxValue;

            for (int i = 0; i < entityData.Length; i++)
            {
                if (entityData[i].isAware && (Vector3.Distance(transform.position, entityData[i].entity.transform.position) < _distance))
                {
                    targetIndex = i;
                    _distance = Vector3.Distance(transform.position, entityData[i].entity.transform.position);
                }
            }
        }

        //Set last seen location
        if (targetIndex != int.MaxValue)
        {
            if (entityData[targetIndex].isAware)
            {
                lastTargetLocation = entityData[targetIndex].entity.transform.position;
            }

            print(entityData[targetIndex].awareness);
        }
    }
    
}
