using UnityEngine;
using System.Collections;

public class Pounce : Ability {

    public float jumpForce;
    public float minJumpTime = 0.1f;
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

        nav.enabled = false;
        GetComponent<Rigidbody>().isKinematic = false;

        GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 1, 5) * jumpForce, ForceMode.Impulse);
        GetComponent<AudioSource>().PlayOneShot(attackSound);
        yield return new WaitForSeconds(minJumpTime);
        _isJumping = true;

        while (_isJumping) 
        {
            yield return new WaitForSeconds(0.1f);
        }

        GetComponent<Rigidbody>().isKinematic = true;
        nav.enabled = true;
    }

    void OnCollisionEnter(Collision other)
    {
        if (_isJumping)
        {
            _isJumping = false;
        }
    }
}
