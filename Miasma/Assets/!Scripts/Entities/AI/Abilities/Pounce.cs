using UnityEngine;
using System.Collections;

public class Pounce : Ability {

    public float pounceSpeed;
    public float pounceTime = 0.1f;
    public float attackConeAngle;
    public AudioClip attackSound;

    private bool _isJumping = false;

    // Use this for initialization
    void Awake()
    {
        nav = GetComponentInChildren<NavMeshAgent>();
        ai = GetComponentInChildren<BaseAI>();
    }

    // Update is called once per frame
    void Update()
    {
        Cooldown();
    }

    // Use Ability
    public override IEnumerator UseAbility()
    {
        cooldownTimer = cooldown;

        yield return new WaitForSeconds(windUpTime);

        GetComponent<AudioSource>().PlayOneShot(attackSound);
        nav.speed = pounceSpeed;
        yield return new WaitForSeconds(pounceTime);
        nav.speed = ai.activeSpeed;
    }

    void OnCollisionEnter(Collision other)
    {
        if (_isJumping)
        {
            _isJumping = false;
        }
    }
}
