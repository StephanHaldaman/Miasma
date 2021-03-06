﻿using UnityEngine;
using System.Collections;

public class BaseAI : Entity {

    public float agro;
    public float activeSpeed;
    public float passiveSpeed;
    public float alertness;

    public float awarenessAgro;
    public float calmRate;
    public float damageAgro;

    public PassiveEffect passiveEffect;

    public Vector3 directionOfInterest;

    public Vector3 lastTargetLocation;
    public int targetIndex = int.MaxValue; //Maybe find a better way of setting a "Null" value
    public float targetDeltaAngle;
    public AwarenessData[] entityData;

    protected AIBehaviour[] behaviours;
    public Ability[] abilities;

    protected NavMeshAgent nav;

    private float _deltaAwareness;
    public int _behaviourIndex;
    public float _behaviourAgro;

    protected void UpdateBehavior()
    {
        _behaviourAgro = 0;
        _behaviourIndex = 0;

        for (int i = 0; i < behaviours.Length; i++)
        {
            if (behaviours[i].agroRequirement <= agro && behaviours[i].agroRequirement > _behaviourAgro)
            {
                _behaviourIndex = i;
                _behaviourAgro = behaviours[i].agroRequirement;
            }
        }

        behaviours[_behaviourIndex].Behave();
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
                    targetDeltaAngle = Mathf.Abs(Mathf.DeltaAngle(Quaternion.LookRotation((entityData[i].entity.transform.position - transform.position)).eulerAngles.y, transform.eulerAngles.y));
                    //Multiply awareness based on view of target
                    _deltaAwareness *= Mathf.Clamp(Mathf.Cos(targetDeltaAngle * Mathf.Deg2Rad), 0, 1);
                    //Don't let awareness past 100
                    entityData[i].awareness = Mathf.MoveTowards(entityData[i].awareness, 100, _deltaAwareness * entityData[i].entity.visibility);

                    //TESTING
                    //print(entityData[i].awareness + "     " + _deltaAwareness / Time.deltaTime);
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

            //Toggle variable so that speed is only set once FIX THIS STUFF
            if (entityData[i].awareness == 100 && entityData[i].isAware != true)
            {
                nav.speed = activeSpeed;
            }
            else if (entityData[i].awareness != 100 && entityData[i].isAware == true)
            {
                nav.speed = passiveSpeed;
            }

            //Set Awareness
            if (entityData[i].awareness == 100)
            {
                entityData[i].isAware = true;
            }
            else
            {
                entityData[i].isAware = false;
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

            //TESTING
            //print(entityData[targetIndex].awareness);
        }
    }

    public void UpdateAgro() 
    {
        // Change agro whether or not target is present
        if (targetIndex != int.MaxValue && entityData[targetIndex].isAware)
        {
            agro = Mathf.MoveTowards(agro, 100, awarenessAgro * Time.deltaTime);
        }
        else
        {
            agro = Mathf.MoveTowards(agro, 0, calmRate * Time.deltaTime);
        }
    }
}
