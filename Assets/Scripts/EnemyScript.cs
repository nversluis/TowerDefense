using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyScript : MonoBehaviour
{
	private GameObject ResourceManagerObj;
	private ResourceManager resourceManager;
	private float nodeSize;
	private List<WayPoint> grid;
	List<Vector3> Path;
    List<Vector3> Path2;
    EnemyHealth enemyHealth;
	int i = 0;
	float normalWalkSpeed;
	float walkSpeed;
	float orcHeigthSpawn = 3.27f;
	CharacterController characterController;
	public bool automaticPathUpdating;
	EnemyStats enemystats;
	List<float> oldList = new List<float> ();
	List<WayPoint> WaypointsNearNow = new List<WayPoint> ();
    List<WayPoint> WaypointsNearOld = new List<WayPoint> ();

    float dfactor;
    bool drawPath;

    public bool walking;
    public bool attacking;

    GameObject player;

	private float speedReduce;
	public bool isSlowed;
	// Use this for initialization
	void Start ()
	{
		isSlowed = false;
        enemystats = GetComponent<EnemyStats>();
		ResourceManagerObj = GameObject.Find ("ResourceManager");
		resourceManager = ResourceManagerObj.GetComponent<ResourceManager> ();
		grid = resourceManager.Nodes;
		float nodeSize = resourceManager.nodeSize;
        drawPath = resourceManager.drawPath;
        normalWalkSpeed = resourceManager.walkSpeed * enemystats.speedMultiplier;
        enemyHealth = GetComponent<EnemyHealth>();
        player = GameObject.Find("Player");


		characterController = GetComponent<CharacterController> ();
        Path = Navigator.Path(transform.position - new Vector3(0f, transform.position.y, 0f), PlayerController.location - new Vector3(0f, PlayerController.location.y, 0f), nodeSize, grid, dfactor);

		//walkSpeed = enemystats.speed/10 + 3;
        dfactor = enemystats.dfactor;

		if (automaticPathUpdating) {
			InvokeRepeating ("BuildPath", 0, 0.5f);
		}

	}

	//Update is called once per frame
	void FixedUpdate ()
	{
		if (isSlowed) {
			speedReduce = resourceManager.speedReduceRate;
		} else {
			speedReduce = 1;
		}
		walkSpeed = normalWalkSpeed * speedReduce;
		if (Path != null) {
			Vector3 dir;

            if (i != Path.Count-1)
            {
                walking = true;
                attacking = false;

                dir = (Path[i + 1] - (transform.position - new Vector3(0f, transform.position.y, 0f))).normalized * walkSpeed;
            }
            else
            {
                dir = (Path[i] - (transform.position - new Vector3(0f, transform.position.y, 0f))).normalized * walkSpeed;
                walking = true;
                attacking = false;

            }

            if (enemyHealth.isDead != true)
            {
                rigidbody.velocity = (dir + new Vector3(0f, rigidbody.velocity.y, 0f));
                rigidbody.angularVelocity = Vector3.zero;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir.normalized), Time.deltaTime * 5f);
                transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);

            }

            else
            {
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;

            }

            Vector3 nextPointDistance = (Path[i] - transform.position - new Vector3(0f, transform.position.y, 0f));
            nextPointDistance.y = 0;

			if (nextPointDistance.magnitude < 1f && i < Path.Count - 1) {
				i++;
			}


			if ((player.transform.position-transform.position).magnitude < 3f) {
				rigidbody.velocity = Vector3.zero;
                walking = false;
                attacking = true;
      
			}

            if (enemyHealth.isDead)
            {
                rigidbody.velocity = Vector3.zero;
                walking = false;
                attacking = false;
                
            }
		}

		if (Input.GetKeyDown (KeyCode.Q) && !automaticPathUpdating) {
			Path = Navigator.Path (transform.position, PlayerController.location, nodeSize, grid);
			i = 0;
		}

		if (Path != null && drawPath) {
			for (int k = 0; k < Path.Count - 1; k++) {
				Debug.DrawLine (Path [k], Path [k + 1]);
			}
		}


        if (Path2 != null && drawPath)
        {
            for (int k = 0; k < Path2.Count - 1; k++)
            {
                Debug.DrawLine(Path2[k], Path2[k + 1], Color.red);
            }
        }
	}

    void BuildPath()
    {
        if (!enemyHealth.isDead)
        {
            Path = Navigator.Path(transform.position - new Vector3(0f, transform.position.y, 0f), PlayerController.location, resourceManager.nodeSize, resourceManager.Nodes, dfactor);
            Path2 = Navigator.Path(transform.position - new Vector3(0f, transform.position.y, 0f), PlayerController.location, resourceManager.nodeSize, resourceManager.Nodes);

            i = 0;
            int j = 0;

            foreach (WayPoint waypoint in WaypointsNearOld)
            {
                waypoint.setPenalty(waypoint.getPenalty() - 5f);
                j = j + 1;
                if (waypoint.getPenalty() < 0)
                    waypoint.setPenalty(0);
            }

            WaypointsNearNow = Navigator.FindWayPointsNear(transform.position, resourceManager.Nodes, resourceManager.nodeSize);
            foreach (WayPoint waypoint in WaypointsNearNow)
            {
                oldList.Add(waypoint.getPenalty());
                waypoint.setPenalty(waypoint.getPenalty() + 5);
            }

            WaypointsNearOld = WaypointsNearNow;
        }
    }


}
