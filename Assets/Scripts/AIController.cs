using UnityEngine;
using System.Collections;

public class AIController : MonoBehaviour {

    NavMeshAgent agent;
    Transform target;

	// Use this for initialization
	void Start () {
       // agent = GetComponent<NavMeshAgent>();
        //target = GameObject.Find("Goal").transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //agent.SetDestination(target.position);
	}
}
