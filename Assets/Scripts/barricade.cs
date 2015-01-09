using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class barricade : MonoBehaviour
{

	private GameObject ResourceManagerObj;
	private ResourceManager resourceManager;
	int cost;
	private PlayerData playerData;
	private GameObject bar;
	// Use this for initialization
	void Start ()
	{
		ResourceManagerObj = GameObject.Find ("ResourceManager");
		resourceManager = ResourceManagerObj.GetComponent<ResourceManager> ();
		cost = resourceManager.costBarricade;
		playerData = GUIScript.player;
		bar = resourceManager.barricade;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) {
			BuildTrap ();
		}	
	}

	void BuildTrap ()
	{
		if (cost <= playerData.getGold ()) {
			GameObject trap = (GameObject)Instantiate (bar, transform.position, Quaternion.identity);//Instantiantion of the tower
			Destroy (trap.GetComponent<barricade> ());
			trap.collider.isTrigger = false;
			trap.gameObject.transform.localScale = new Vector3 (1, 1, 1) * resourceManager.planewidth * 1.7f / 20;
			trap.transform.parent = gameObject.transform.parent;
			trap.tag = "Tower";
			trap.layer = 13;
			trap.SetActiveRecursively (true); 
			playerData.addGold (-cost);
			setPenalties ();
			WallScript.DestroyHotSpots ();
			//Destroy (gameObject);
		} else {

		}
	}


	void setPenalties ()
	{
		List<WayPoint> wayPointsNear = new List<WayPoint> ();
		for (int x = -1; x <= 1; x += 2) {
			for (int y = -1; y <= 1; y += 2) {
				List<WayPoint> wayPoints = Navigator.FindWayPointsNear (transform.position + new Vector3 (x, 0, y), resourceManager.Nodes, resourceManager.nodeSize);
				foreach (WayPoint point in wayPoints) {
					if (!wayPointsNear.Contains (point)) {
						wayPointsNear.Add (point);
						if (point != null) {
							point.setPenalty (500);
						}
						else{
							resourceManager.Nodes.Remove (point);
						}
					}
				}
			}
		}
	}

}
