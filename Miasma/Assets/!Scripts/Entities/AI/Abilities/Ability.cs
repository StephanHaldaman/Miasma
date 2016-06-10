using UnityEngine;
using System.Collections;

public abstract class Ability : MonoBehaviour {

    public float windUpTime;
    public float cooldown;
    public float agroCost;
    public float range;

    public float cooldownTimer;

    protected NavMeshAgent nav;
    protected BaseAI ai;

    // Use Ability
    public abstract IEnumerator UseAbility();

    // Cooldown Ability
    public void Cooldown()
    {
        cooldownTimer = Mathf.MoveTowards(cooldownTimer, 0, Time.deltaTime);
    }
}
