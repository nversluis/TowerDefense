using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GuyantScript : MonoBehaviour
{
    EnemyHealth enemyHealth;
    EnemyStats enemystats;
    ResourceManager resourceManager;
    EnemyResources enemyResources;

	GameObject ResourceManagerObj;
    GameObject player;
    GameObject goal;
    GameObject Target;

	List<WayPoint> grid;
    List<WayPoint> WaypointsNearNow = new List<WayPoint>();
    List<WayPoint> WaypointsNearOld = new List<WayPoint>();

	List<Vector3> Path;
    List<Vector3> Path2;

    List<float> oldList = new List<float>();

    float nodeSize;
    float normalWalkSpeed;
    float walkSpeed;
    float dfactor;
    float pathUpdateRate;
    float penalty = 5f;
    float speedReduce;

    int i = 0;

    bool drawPath;
    bool automaticPathUpdating;


    public bool isSlowed = false;

    // Method for finding all necessary scripts
    void GetScripts()
    {
        // Getting other scripts from this enemy
        enemystats = GetComponent<EnemyStats>();
        enemyHealth = GetComponent<EnemyHealth>();
        enemyResources = GetComponent<EnemyResources>();

        // Getting the ResourceManger with script
        ResourceManagerObj = GameObject.Find("ResourceManager");
        resourceManager = ResourceManagerObj.GetComponent<ResourceManager>();

        // Finding the player and the goal
        player = GameObject.Find("Player");
        goal = GameObject.Find("Goal");

        // getting all variables from other scripts
        grid = resourceManager.Nodes;
        nodeSize = resourceManager.nodeSize;
        drawPath = resourceManager.drawPath;
        normalWalkSpeed = resourceManager.walkSpeed * enemystats.speedMultiplier;
        pathUpdateRate = resourceManager.pathUpdateRate;
        dfactor = enemystats.dfactor;
        automaticPathUpdating = resourceManager.automaticPathUpdating;
        

    }

    // Method for determining the speed of the enemy
    void WalkSpeed()
    {

        // if the enemy is slowed
        if (enemyResources.isSlowed)
        {
            // reduce speed
            speedReduce = resourceManager.speedReduceRate;
        }
        else
        {
            // else speed is is normal speed;
            speedReduce = 1;
        }
        walkSpeed = normalWalkSpeed * speedReduce;
    }

    // Method for the movement of the enemy
    void Moving()
    {
        // if enemy found a path to its current goal
        if (Path != null) {
			Vector3 dir;
            // if the enemy is not near its goal
            if (i < Path.Count-1)
            {

                // the enemy is walking and not attacking
                enemyResources.walking = true;
                enemyResources.attacking = false;

                // walk to the next point to smooth the walking of the enemy
                dir = (Path[i + 1] - (transform.position - new Vector3(0f, transform.position.y, 0f))).normalized * walkSpeed;
            }
            else
            {
                // else walk to the current point
                dir = (Path[i] - (transform.position - new Vector3(0f, transform.position.y, 0f))).normalized * walkSpeed;
                enemyResources.walking = true;
                enemyResources.attacking = false;

            }

            // if the enemy is not dead
            if (enemyResources.isDead != true)
            {
                // move in the direction of the next point
                rigidbody.velocity = (dir + new Vector3(0f, rigidbody.velocity.y, 0f));

                // set angular velocity to zero
                rigidbody.angularVelocity = Vector3.zero;

                // rotate in the direction of where it is walking
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir.normalized), Time.deltaTime * 5f);

                // set rotations in other directions to zero
                transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);

            }
            else
            {
                // when dead do not move
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;

            }

            // when not close to the goal
            if (i < Path.Count - 1)
            {
                // determine the distance to the next point in only x and z
                Vector3 nextPointDistance = (Path[i + 1] - transform.position - new Vector3(0f, transform.position.y, 0f));
                nextPointDistance.y = 0;

                // if the distance is smaller than 1 increase i by 1
                if (nextPointDistance.magnitude < 2f)
                {
                    i++;
                }
            }
            // when close to the goal
            else
            {
                // determine the distance to the next point in only x and z
                Vector3 nextPointDistance = (Path[i] - transform.position - new Vector3(0f, transform.position.y, 0f));
                nextPointDistance.y = 0;

            }

            // if the player is near the enemy attack the player
		    if ((player.transform.position-transform.position).magnitude < 3f) {

                // set speed to zero and attack
			    rigidbody.velocity = Vector3.zero;
                enemyResources.walking = false;
                enemyResources.attacking = true;
		    }

            // when enemy is dead
            if (enemyResources.isDead)
            {
                // set speed to zero
                rigidbody.velocity = Vector3.zero;
                enemyResources.walking = false;
                enemyResources.attacking = false;
            }

            // when enemy reaches the end
            if ((goal.transform.position - transform.position).magnitude < 2f)
            {
                // enemy has won
                GoalScript.lives--;

                // destroy it
                Destroy(this.gameObject);
            }
	    }
    }

    // Method for choosing a target
    void DetermineTarget()
    {
        // checking whether player or goal is more interesting
        if ((transform.position - player.transform.position).magnitude/enemystats.playerImportance < (transform.position-goal.transform.position).magnitude/enemystats.goalImportance){
            Target = player;
        }
        else
        {
            Target = goal;
        }
    }

    // Method for debugging purposes
    void Debuging()
    {
        // when automatic path updating is off and q is pressed the path is updated
        if (Input.GetKeyDown(KeyCode.Q) && !automaticPathUpdating)
        {
            Path = Navigator.Path(transform.position, PlayerController.location, nodeSize, grid);
            i = 0;
        }

        // When draw path is enabled draw the path with own dfactor
        if (Path != null && drawPath)
        {
            for (int k = 0; k < Path.Count - 1; k++)
            {
                Debug.DrawLine(Path[k], Path[k + 1]);
            }
        }

        // when draw path is enabled draw the path without dfactor
        if (Path2 != null && drawPath)
        {
            for (int k = 0; k < Path2.Count - 1; k++)
            {
                Debug.DrawLine(Path2[k], Path2[k + 1], Color.red);
            }
        }
    }

	// Use this for initialization
	void Start ()
	{
        // Getting all necessary scripts
        GetScripts();

        // Repeat the pathfinding process
		if (automaticPathUpdating) {
            Target = goal;
			InvokeRepeating ("BuildPath", 0, pathUpdateRate);
		}
        else
        {
            Target = goal;
            BuildPath();
        }

	}

	//Update is called once per frame
	void FixedUpdate ()
	{

        // Determine the walk speed of the enemy
        WalkSpeed();

        // enemy movement
        Moving();
        
        // Determine the target
        DetermineTarget();

        // Debug
        Debuging();

	}

    void BuildPath()
    {
        // When enemy is not dead
        if (!enemyResources.isDead)
        {
            // determine a path to a goal
            Path = Navigator.Path(transform.position - new Vector3(0f, transform.position.y, 0f), Target.transform.position - new Vector3(0f, Target.transform.position.y, 0f), nodeSize, grid, dfactor);
            // if drawPath is enabled also calculate a second path without dfactor
            if (drawPath)
            {
                Path2 = Navigator.Path(transform.position - new Vector3(0f, transform.position.y, 0f), Target.transform.position - new Vector3(0f, Target.transform.position.y, 0f), nodeSize, grid);
            }

            // set i back to 0;
            i = 0;

            // for each waypoint from previous update set the penalty back to original
            foreach (WayPoint waypoint in WaypointsNearOld)
            {
                waypoint.setPenalty(waypoint.getPenalty() - penalty);

                // if penalty is lower than 0 set it to 0
                if (waypoint.getPenalty() < 0)
                    waypoint.setPenalty(0);
            }

            // Find waypoints that are close
            WaypointsNearNow = Navigator.FindWayPointsNear(transform.position, resourceManager.Nodes, resourceManager.nodeSize);

            // for each waypoint it is close to now set a penalty
            foreach (WayPoint waypoint in WaypointsNearNow)
            {
                waypoint.setPenalty(waypoint.getPenalty() + penalty);
            }

            // set new waypoints to old for next update
            WaypointsNearOld = WaypointsNearNow;
        }
    }


}
