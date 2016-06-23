using UnityEngine;
using System.Collections;

public class AIRoam : AIBehaviour
{

    public float searchRadius;

    public Transform[] roamPoints;
    public int roamIndex = 0;

    private bool _hasRoamingNodes = false;
    private RoamingNode[] _roamingNodes;
    private Transform[] _newRoamPoints;

	// Use this for initialization
    void Awake()
    {
        inCombatOnly = false;
        nav = GetComponentInChildren<NavMeshAgent>();
        ai = GetComponent<BaseAI>();
	}
	
	// Use Behaviour
    public override void Behave()
    {
        if (!_hasRoamingNodes)
        {
            SearchForNodes();
        }

        if (nav.remainingDistance <= nav.stoppingDistance)
        {
            // Check if there are no more patrol points
            if (roamIndex != roamPoints.Length)
            {
                nav.SetDestination(roamPoints[roamIndex].position);
                roamIndex++;
            }
            else
            {
                _hasRoamingNodes = false;
                roamIndex = 0;
            }
        }
	}

    private void SearchForNodes() 
    {
        //Clear roaming points
        roamPoints = new Transform[1];
        //Find roaming nodes
        _roamingNodes = GameObject.FindObjectsOfType<RoamingNode>();

        for (int i = 0; i < _roamingNodes.Length; i++)
        {
            if (Vector3.Distance(_roamingNodes[i].transform.position, transform.position) <= searchRadius)
            {
                if (roamPoints[0] == null)
                {
                    //Set first node
                    roamPoints[0] = _roamingNodes[0].transform;
                }
                else
                {
                    _newRoamPoints = new Transform[roamPoints.Length + 1];

                    for (int j = 0; j < roamPoints.Length; j++)
                    {
                        _newRoamPoints[j] = roamPoints[j];
                    }

                    _newRoamPoints[roamPoints.Length] = _roamingNodes[i].transform;
                    roamPoints = _newRoamPoints;
                }
            }
        }

        if (roamPoints.Length > 0)
        {
            _hasRoamingNodes = true;
        }
    }

    //Sort from closest to farthest
    private void SortTransform()
    {
            
    }
}
