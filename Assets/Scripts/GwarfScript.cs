using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GwarfScript : MonoBehaviour
{
	EnemyHealth enemyHealth;
	private float timeBetweenAttacks = 2.917f / 2f;
	EnemyStats enemystats;
	ResourceManager resourceManager;
	EnemyResources enemyResources;

	GameObject ResourceManagerObj;
	GameObject player;
	GameObject goal;
	GameObject Target;
	List<Vector3> barricades;

	GoalScript goalScript;

	List<WayPoint> grid;
	List<WayPoint> WaypointsNearNow = new List<WayPoint> ();
	List<WayPoint> WaypointsNearOld = new List<WayPoint> ();

	List<Vector3> Path;
	List<Vector3> Path2;

	List<float> oldList = new List<float> ();

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
    bool invoked;

	public float isSlowed = 1;

	Vector3 goalLoc = new Vector3 ();

	private Vector3 curTarget;
	public bool attackingGoal;
	public bool attackingBar;

	public bool throughGate;

	GameObject curFloor;
	bool isInvoked;

    AudioClip walking;
    float volume;
	// Method for finding all necessary scripts
	void GetScripts ()
	{
		// Getting other scripts from this enemy
		enemystats = GetComponent<EnemyStats> ();
		enemyHealth = GetComponent<EnemyHealth> ();
		enemyResources = GetComponent<EnemyResources> ();

		// Getting the ResourceManger with script
		ResourceManagerObj = GameObject.Find ("ResourceManager");
		resourceManager = ResourceManagerObj.GetComponent<ResourceManager> ();

		// Finding the player and the goal
		player = GameObject.Find ("Player");
		goal = GameObject.Find ("Goal");
		goalScript = goal.GetComponent<GoalScript> ();

		// getting all variables from other scripts
		grid = resourceManager.Nodes;
		nodeSize = resourceManager.nodeSize;
		drawPath = resourceManager.drawPath;
		normalWalkSpeed = resourceManager.walkSpeed * enemystats.speedMultiplier;
		pathUpdateRate = resourceManager.pathUpdateRate;
		dfactor = enemystats.dfactor;
		automaticPathUpdating = resourceManager.automaticPathUpdating;

		goalLoc = GameObject.Find ("Goal").transform.position - new Vector3 (0, -5, 0);

        walking = resourceManager.walking;
        volume = (float)PlayerPrefs.GetInt("SFX")/100f;
	}

	// Method for determining the speed of the enemy
	void WalkSpeed ()
	{
		speedReduce = enemyResources.isSlowed;
		walkSpeed = normalWalkSpeed / speedReduce;
	}

	// Method for the movement of the enemy
	void Moving ()
	{
		// if enemy found a path to its current goal
		if (Path != null) {
			Vector3 dir;
			// if the enemy is not near its goal
			if (i < Path.Count - 1) {

				if (!enemyResources.isDead) {
					// the enemy is walking and not attacking
					enemyResources.walking = true;
					enemyResources.attacking = false;
				}

				// walk to the next point to smooth the walking of the enemy
				dir = (Path [i] - (transform.position - new Vector3 (0f, transform.position.y, 0f))).normalized * walkSpeed;
			} else {
				// else walk to the current point
				dir = (Path [i] - (transform.position - new Vector3 (0f, transform.position.y, 0f))).normalized * walkSpeed;
				if (!enemyResources.isDead) {
					// the enemy is walking and not attacking
					enemyResources.walking = true;
					enemyResources.attacking = false;
				}

			}

			// if the enemy is not dead
			if (!enemyResources.isDead) {
				// move in the direction of the next point
				rigidbody.velocity = (dir + new Vector3 (0f, rigidbody.velocity.y, 0f));

				// set angular velocity to zero
				rigidbody.angularVelocity = Vector3.zero;

				// rotate in the direction of where it is walking
				transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler (Quaternion.LookRotation (dir.normalized).eulerAngles + new Vector3 (0, 270, 0)), Time.deltaTime * 5f);

				// set rotations in other directions to zero
				transform.rotation = Quaternion.Euler (0f, transform.rotation.eulerAngles.y, 0f);


			} else {
				// when dead do not move
				rigidbody.velocity = Vector3.zero;
				rigidbody.angularVelocity = Vector3.zero;

			}

			// when not close to the goal
			if (i < Path.Count - 1) {
				// determine the distance to the next point in only x and z
				Vector3 nextPointDistance = (Path [i] - transform.position - new Vector3 (0f, transform.position.y, 0f));
				nextPointDistance.y = 0;

				// if the distance is smaller than 1 increase i by 1
				if (nextPointDistance.magnitude < 2f) {
					i++;
				}
			}
            // when close to the goal
            else {
				// determine the distance to the next point in only x and z
				Vector3 nextPointDistance = (Path [i] - transform.position - new Vector3 (0f, transform.position.y, 0f));
				nextPointDistance.y = 0;

			}
			if (throughGate) {
				RaycastHit hit;
				RaycastHit goalHit;
				Physics.Raycast (transform.position, player.transform.position + new Vector3 (0f, 2f, 0f) - transform.position, out hit);
				Physics.Raycast (transform.position, goalLoc- transform.position, out goalHit);
				Debug.DrawRay (transform.position, goalLoc - transform.position);
				if ((goalLoc - transform.position).magnitude < 20f && goalHit.transform.name == "Goal") {

					// set speed to zero and attack
					enemyResources.walking = false;
					enemyResources.attacking = true;
					rigidbody.velocity = Vector3.zero;
					transform.LookAt (new Vector3 (goalLoc.x, transform.position.y, goalLoc.z));
					transform.Rotate (0, -90, 0);
					if (!attackingGoal) {
						curTarget = goalLoc;
						attackingGoal = true;
						attackingBar = false;
						InvokeRepeating ("Shoot", timeBetweenAttacks / 1.5f, timeBetweenAttacks);

					}
			
				} else if ((player.transform.position - transform.position).magnitude < 30f && Target.Equals (player) && hit.transform.name == "Player") { // if the player is near the enemy attack the player
					CancelInvoke ("Shoot");
					attackingGoal = false;
					attackingBar = false;
					transform.LookAt (new Vector3 (player.transform.position.x, transform.position.y, player.transform.position.z));
					transform.Rotate (0, -90, 0);
					// set speed to zero and attack
					rigidbody.velocity = Vector3.zero;
					enemyResources.walking = false;
					enemyResources.attacking = true;
				} else {
					foreach (Vector3 barricade in barricades) {
						if ((barricade - transform.position).magnitude < 30f) {
							Physics.Raycast (transform.position, barricade - transform.position, out hit);
							if (hit.transform.position == barricade) {
								// set speed to zero and attack
								rigidbody.velocity = Vector3.zero;
								enemyResources.walking = false;
								enemyResources.attacking = true;
								transform.LookAt (new Vector3 (goalLoc.x, transform.position.y, goalLoc.z));
								transform.Rotate (0, -90, 0);
								if (!attackingBar) {
									curTarget = barricade;
									attackingBar = true;
									InvokeRepeating ("Shoot", timeBetweenAttacks / 1.5f, timeBetweenAttacks);
								}
								break;
							}
						}
					}
					if (!attackingBar) {
						CancelInvoke ("Shoot");
						attackingGoal = false;
						attackingBar = false;
					}
				}
			}


			// when enemy is dead
			if (enemyResources.isDead) {
				// set speed to zero
				try
				{
					curFloor.GetComponent<FloorScript>().hasEnemy = false;
				}
				catch
				{
					Debug.Log("buiten de gate gekilled");
				}
				rigidbody.velocity = Vector3.zero;
				enemyResources.walking = false;
				enemyResources.attacking = false;
			}

			// when enemy reaches the end
			if ((goal.transform.position - transform.position).magnitude < 2f) {
				// enemy has won
				goalScript.removeLife ();

				// destroy it
				Destroy (this.gameObject);
				// GetComponent<EnemyResources>().isDead = true;
				// this.transform.position = GetComponent<EnemyHealth>().deathPosition;
			}
		}
			
	}

	// Method for choosing a target
	void DetermineTarget ()
	{
		// checking whether player or goal is more interesting
		if ((transform.position - player.transform.position).magnitude / enemystats.playerImportance < (transform.position - goal.transform.position).magnitude / enemystats.goalImportance) {
			Target = player;
		} else {
			Target = goal;
		}
	}

	// Method for debugging purposes
	void Debuging ()
	{
		// when automatic path updating is off and q is pressed the path is updated
		if (Input.GetKeyDown (KeyCode.Q) && !automaticPathUpdating) {
			List<WayPoint> WPPath = Navigator.Path (transform.position, PlayerController.location, nodeSize, grid);
			if (WPPath != null) {
				Path = new List<Vector3> ();
				foreach (WayPoint wp in WPPath) {
					Path.Add (wp.getPosition ());
				}
			}
			i = 0;
		}

		// When draw path is enabled draw the path with own dfactor
		if (Path != null && drawPath) {
			for (int k = 0; k < Path.Count - 1; k++) {
				Debug.DrawLine (Path [k], Path [k + 1]);
			}
		}

		// when draw path is enabled draw the path without dfactor
		if (Path2 != null && drawPath) {
			for (int k = 0; k < Path2.Count - 1; k++) {
				Debug.DrawLine (Path2 [k], Path2 [k + 1], Color.red);
			}
		}
	}

	void Shoot ()
	{
		enemyResources.walking = false;
		enemyResources.attacking = true;
		Vector3 targetLoc = curTarget;
		GwarfAttack ga = gameObject.GetComponent<GwarfAttack> ();
		GameObject Bullet = (GameObject)Instantiate (ga.bullet, transform.position, Quaternion.identity);
		Bullet.GetComponent<GwarfBulletScript> ().gwarf = gameObject;
		Bullet.GetComponent<GwarfBulletScript> ().damagePerShot = ga.attackDamage;
		Vector3 dir = (targetLoc - transform.position).normalized;
		Bullet.rigidbody.velocity = ga.bulletSpeed * dir;
	}


	// Use this for initialization
	void Start ()
	{
		throughGate = false;
		isInvoked = false;
		// Getting all necessary scripts
		GetScripts ();

		// BuildPath
		Target = goal;
		BuildPath ();

		enemyResources.isSlowed = 1;
		attackingGoal = false;
	}

    void Walking()
    {
        audio.PlayOneShot(walking, volume);
    }

	//Update is called once per frame
	void FixedUpdate ()
	{
		if (!isInvoked &&throughGate) {
			Target = goal;
			InvokeRepeating ("BuildPath", 0, pathUpdateRate);
			isInvoked = true;
		}

		// When draw path is enabled draw the path with own dfactor
		if (Path != null && drawPath) {
			for (int k = 0; k < Path.Count - 1; k++) {
				Debug.DrawLine (Path [k], Path [k + 1]);
			}
		}

		// when draw path is enabled draw the path without dfactor
		if (Path2 != null && drawPath) {
			for (int k = 0; k < Path2.Count - 1; k++) {
				Debug.DrawLine (Path2 [k], Path2 [k + 1], Color.red);
			}
		}
		checkFloor ();

		// Determine the walk speed of the enemy
		WalkSpeed ();

		// enemy movement
		Moving ();

		// Determine the target
		DetermineTarget ();

		// Debug
		Debuging ();

        if (enemyResources.walking && !invoked)
        {
            InvokeRepeating("Walking", 0f, 1.097f);
            invoked = true;
        }
        if (!enemyResources.walking)
        {
            invoked = false;
            CancelInvoke("Walking");
        }

	}

	void BuildPath ()
	{
		// When enemy is not dead
		if (!enemyResources.isDead) {
			// determine a path to a goal
			List<WayPoint> WPPath = Navigator.Path (transform.position - new Vector3 (0f, transform.position.y, 0f), Target.transform.position - new Vector3 (0f, Target.transform.position.y, 0f), nodeSize, grid, dfactor);
			barricades = new List<Vector3> ();
			if (WPPath != null) {
				Path = new List<Vector3> ();
				foreach (WayPoint wp in WPPath) {
					Path.Add (wp.getPosition ());
					if (wp.getBarCount () > 0) {
						barricades.Add (wp.getBarricade ());
					}
				}
			}
			// if drawPath is enabled also calculate a second path without dfactor
			if (drawPath) {
				List<WayPoint> WPPath2 = Navigator.Path (transform.position - new Vector3 (0f, transform.position.y, 0f), Target.transform.position - new Vector3 (0f, Target.transform.position.y, 0f), nodeSize, grid);
				if (WPPath2 != null) {
					Path2 = new List<Vector3> ();
					foreach (WayPoint wp in WPPath2) {
						Path2.Add (wp.getPosition ());
					}
				}
			}

			// set i back to 0;
			i = 0;

			// for each waypoint from previous update set the penalty back to original
			foreach (WayPoint waypoint in WaypointsNearOld) {
				try {
					waypoint.setPenalty (waypoint.getPenalty () - penalty);

					// if penalty is lower than 0 set it to 0
					if (waypoint.getPenalty () < 0)
						waypoint.setPenalty (0);
				} catch {
				}
			}

			// Find waypoints that are close
			WaypointsNearNow = Navigator.FindWayPointsNear (transform.position, resourceManager.Nodes, resourceManager.nodeSize);

			// for each waypoint it is close to now set a penalty
			foreach (WayPoint waypoint in WaypointsNearNow) {
				try {
					waypoint.setPenalty (waypoint.getPenalty () + penalty);
				} catch {

				}
			}

			// set new waypoints to old for next update
			WaypointsNearOld = WaypointsNearNow;
		}
	}

	void checkFloor ()
	{
		RaycastHit hit;
		GameObject res = curFloor;
		//Ray ray = new Ray(transform.position, -Vector3.up, out hit);
		if (Physics.Raycast (transform.position, -Vector3.up, out hit)) {
			if (hit.transform.name.Contains ("loor")) {
				curFloor = hit.transform.gameObject;
				if (res != null && res != curFloor) {
					res.GetComponent<FloorScript> ().hasEnemy = false;
					if (curFloor.transform.childCount == 2)
						WallScript.DestroyHotSpots ();
				}
				FloorScript floor = hit.transform.GetComponent<FloorScript> ();
				floor.hasEnemy = true;
			}
		}
	}

}
