using UnityEngine;
using System.Collections;

public class AIController : MonoBehaviour {

    NavMeshAgent agent;
    Transform target;

    void OnCollisionEnter(Collision other){

        if(other.gameObject.tag == "Bullet"){
            Destroy(this.gameObject);
        }

    }

	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {

	
	}
}
