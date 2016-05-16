using UnityEngine;
using System.Collections;

public class BaseAI : Entity {

    public float agro;
    public float activeSpeed;
    public float passiveSpeed;
    public float alertness;
    public PassiveEffect passiveEffect;
    public GameObject[] abilities;

    public Vector3 directionOfInterest;

    public Vector3 lastTargetLocation;
    public int targetIndex = int.MaxValue; //Maybe find a better way of setting a "Null" value
    public AwarenessData[] entityData;
    
    protected AIBehaviour[] behaviours;

    protected NavMeshAgent nav;

    private float _deltaAwareness;

    protected void UpdateBehavior()
    {
        //WORK IN PROGRESS
        if (targetIndex != int.MaxValue)
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
                if (hit.collider.GetComponentInParent<Entity>() == entityData[i].entity)
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
            else
            {
                //Flatline change in awareness
                _deltaAwareness = 0;
            }

            //Deplete Awareness
            if (_deltaAwareness <= 0)
            {
                entityData[i].awareness = Mathf.MoveTowards(entityData[i].awareness, 0, 2 * Time.deltaTime);
            }
        }
    }

    public void UpdateTarget() 
    {
        //Find closest target if no target is available
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

        if (targetIndex != int.MaxValue)
        {

            //Set last seen location
            if (entityData[targetIndex].isAware)
            {
                lastTargetLocation = entityData[targetIndex].entity.transform.position;
            }

            //Remove target if entity is no longer aware of it
            if (entityData[targetIndex].awareness <= 0)
            {
                targetIndex = int.MaxValue;
            }

            print(entityData[targetIndex].awareness);
        }
    }
    
}
