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
	private GameObject bluetrap;
	bool mouseOver;
	private KeyInputManager inputManager;
	// Use this for initialization
	void Start ()
	{
		ResourceManagerObj = GameObject.Find ("ResourceManager");
		resourceManager = ResourceManagerObj.GetComponent<ResourceManager> ();
		cost = resourceManager.costBarricade;
		playerData = GUIScript.player;
		bar = resourceManager.barricade;
		mouseOver = false;
		inputManager = GameObject.Find ("KeyInputs").GetComponent<KeyInputManager> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) {
			if (tag != "Tower") {
				BuildTrap ();
			}	
		}
		if (gameObject == CameraController.hitObject && tag == "Tower" && WeaponController.weapon == 50) {
			if (!mouseOver) {
				bluetrap = (GameObject)Instantiate (bar, transform.position, Quaternion.identity);
				mouseOver = true;
				Destroy (bluetrap.GetComponent<barricade> ());
				bluetrap.collider.isTrigger = false;
				bluetrap.gameObject.transform.localScale = new Vector3 (1, 1, 1) * resourceManager.planewidth * 1.7f / 20;
				bluetrap.transform.parent = gameObject.transform.parent;
				bluetrap.tag = "Tower";
				bluetrap.layer = 0;
				bluetrap.SetActiveRecursively (true); 
				foreach (Renderer child in bluetrap.GetComponentsInChildren<Renderer>()) {
					child.material.color = new Color (0, 0, 255, 0.1f);
					child.material.shader = Shader.Find ("Transparent/Diffuse");
				}
			}
			if (Input.GetKeyDown (inputManager.upgradeInput)) {
				Upgrade ();
			} else if (Input.GetKeyDown (inputManager.sellInput)) {
				RemoveTrap ();
			}				
		}
		else {
			RemoveBT();
		}
	}

	void BuildTrap ()
	{
		if (cost <= playerData.getGold ()) {
			GameObject trap = (GameObject)Instantiate (bar, transform.position, Quaternion.identity);//Instantiantion of the tower
			barricade barScript = trap.GetComponent<barricade> ();
			trap.collider.isTrigger = false;
			trap.gameObject.transform.localScale = new Vector3 (1, 1, 1) * resourceManager.planewidth * 1.7f / 20;
			trap.transform.parent = gameObject.transform.parent;
			trap.tag = "Tower";
			trap.layer = 0;
			trap.SetActiveRecursively (true); 
			playerData.addGold (-cost);
			setPenalties (500);
			WallScript.DestroyHotSpots ();
			//Destroy (gameObject);
		} else {

		}
	}


	void setPenalties (int penalty)
	{
		List<WayPoint> wayPointsNear = new List<WayPoint> ();
		for (int x = -1; x <= 1; x += 2) {
			for (int y = -1; y <= 1; y += 2) {
				List<WayPoint> wayPoints = Navigator.FindWayPointsNear (transform.position + new Vector3 (x, 0, y), resourceManager.Nodes, resourceManager.nodeSize);
				foreach (WayPoint point in wayPoints) {
					if (!wayPointsNear.Contains (point)) {
						wayPointsNear.Add (point);
						if (point != null) {
							if (penalty > 0) {
								point.setBarricade (1); 
								point.setPenalty (penalty);
							}
							else
							{
								point.setBarricade (-1); 
								if (point.getBarricade () == 0) {
									point.setPenalty (penalty);
								}
							}
						}
						else{
							resourceManager.Nodes.Remove (point);
						}
					}
				}
			}
		}
	}

	void RemoveBT()
	{
		if (bluetrap != null) {
			Destroy (bluetrap);
			mouseOver = false;
		}
	}

	void Upgrade(){
	}

	void RemoveTrap(){
		RemoveBT ();
		setPenalties (0);
		Destroy (gameObject);
	}

}
