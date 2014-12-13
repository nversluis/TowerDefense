using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyScript : MonoBehaviour {

	private GameObject ResourceManagerObj;
	private ResourceManager resourceManager;
	private float nodeSize;
	private List<WayPoint> grid;
    List<Vector3> Path;
    int i = 0;
    float walkSpeed;
    float orcHeigthSpawn = 3.27f;
    CharacterController characterController;
    public bool automaticPathUpdating;
    EnemyStats enemystats;

    
	// Use this for initialization
	void Start () {
		ResourceManagerObj = GameObject.Find ("ResourceManager");
		resourceManager = ResourceManagerObj.GetComponent<ResourceManager> ();
		grid = resourceManager.Nodes;
		float nodeSize = resourceManager.nodeSize;

        characterController = GetComponent<CharacterController>();
		Path = Navigator.Path(new Vector3(transform.position.x,0,transform.position.z), PlayerController.location - new Vector3(0f, PlayerController.location.y,0f),nodeSize,grid);
	
        enemystats = GetComponent<EnemyStats>();
        walkSpeed = enemystats.speed/10 + 3;


        if (automaticPathUpdating)
        {
            InvokeRepeating("BuildPath", 0, 0.5f);
        }

	}

	 //Update is called once per frame
    void Update()
    {

        if (Path != null)
        {
            Vector3 dir;
			if (i != 0) {
				dir = (Path [i] - transform.FindChild ("Floor").position).normalized * walkSpeed;


			} else if (Path.Count != 1) {
				dir = (Path [i + 1] - transform.FindChild ("Floor").position).normalized * walkSpeed;

			} else
				dir = Vector3.zero;

            rigidbody.velocity = (dir);
            rigidbody.angularVelocity = Vector3.zero;
			if (dir != Vector3.zero) {
				transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.LookRotation (dir.normalized), Time.deltaTime * 5f);
				transform.rotation = Quaternion.Euler (0f, transform.rotation.eulerAngles.y, 0f);
			}
            if ((Path[i] - transform.FindChild("Floor").position).magnitude < 1f && i < Path.Count - 1)
            {
                i++;
            }

            if ((Path[i] - transform.FindChild("Floor").position).magnitude < 1f && i == Path.Count - 1)
            {
                rigidbody.velocity = Vector3.zero;

            }
        }


        if (Input.GetKeyDown(KeyCode.Q) && !automaticPathUpdating)
        {
			Path = Navigator.Path(transform.FindChild("Floor").transform.position, PlayerController.location - new Vector3(0f, PlayerController.location.y, 0f),nodeSize,grid);
            i = 0;
        }

    }

	void BuildPath(){

		Path = Navigator.Path(transform.FindChild("Floor").transform.position, PlayerController.location - new Vector3(0f, PlayerController.location.y, 0f),resourceManager.nodeSize,resourceManager.Nodes);
		i = 0;

	}
   
}
