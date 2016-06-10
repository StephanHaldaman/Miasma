using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour {

    public float animationSpeed;
    public AnimationJoint[] joints;

    private NavMeshAgent _nma;

	// Use this for initialization
	void Awake () {
        _nma = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < joints.Length; i++)
        {
            joints[i].UpdateJointPosition(_nma.velocity.magnitude * animationSpeed);
        }
	}
}
