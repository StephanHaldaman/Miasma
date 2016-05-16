using UnityEngine;
using System.Collections;

public abstract class AIBehaviour : MonoBehaviour
{
    public float agroRequirement;
    protected bool inCombatOnly;
    protected NavMeshAgent nav;
    protected BaseAI ai;

    // Use Behaviour
    public abstract void Behave();
}
