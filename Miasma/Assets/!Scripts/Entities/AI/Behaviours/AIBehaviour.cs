using UnityEngine;
using System.Collections;

public abstract class AIBehaviour : MonoBehaviour
{
    public bool inCombatOnly;
    public float agroRequirement;
    protected NavMeshAgent nav;
    protected BaseAI ai;

    // Use Behaviour
    public abstract void Behave();
}
