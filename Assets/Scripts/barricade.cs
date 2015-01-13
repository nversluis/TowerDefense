using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class barricade : MonoBehaviour
{

	private GameObject ResourceManagerObj;
	private ResourceManager resourceManager;
	public int cost;
	public int totalCost;
	private PlayerData playerData;
	private GameObject bar;
	private GameObject bluetrap;
	bool mouseOver;
	private KeyInputManager inputManager;
	public int maxHealth;
	public int health;


	// Use this for initialization
	void Start ()
	{
		ResourceManagerObj = GameObject.Find ("ResourceManager");
		resourceManager = ResourceManagerObj.GetComponent<ResourceManager> ();
		cost = resourceManager.costBarricade;
		totalCost = cost;
		playerData = GUIScript.player;
		bar = resourceManager.barricade;
		mouseOver = false;
		inputManager = GameObject.Find ("KeyInputs").GetComponent<KeyInputManager> ();
		maxHealth = resourceManager.barricadeHealth;
		health = maxHealth;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) {
			if (tag != "Tower" && !transform.parent.gameObject.GetComponent<FloorScript>().hasEnemy) {
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
			} else if (Input.GetKeyDown (inputManager.sellInput)) { //selling the barricade
				RemoveTrap ();
				playerData.addGold (totalCost/2);
			}				
		}
		else {
			RemoveBT();
		}


		if (health <= 0) {
			RemoveTrap ();
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
			trap.layer = 8;
			trap.SetActiveRecursively (true); 
			playerData.addGold (-cost);
			resourceManager.allBarricades.Add (trap);
			setPenalties (health/5);
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
				List<WayPoint> wayPoints = Navigator.FindWayPointsNear (transform.position + new Vector3 (x, -transform.position.y, y), resourceManager.Nodes, resourceManager.nodeSize);
				foreach (WayPoint point in wayPoints) {
					if (!wayPointsNear.Contains (point)) {
						wayPointsNear.Add (point);
						if (point != null) {
							if (penalty > 0) {
								point.setBarricade (transform.position); 
								point.setPenalty (point.getBarCount()*penalty);
							}
							else
							{
								point.removeBarricade (); 
								if (point.getBarCount() == 0) {
									point.setPenalty (penalty);
								}
							}
							Debug.Log (point.getPenalty());
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
		if (maxHealth != health && (maxHealth-health)<=playerData.getGold()) {
			playerData.addGold (-(maxHealth - health));
			totalCost += (maxHealth - health);
			health = maxHealth;
		} else {
			if(cost*maxHealth/resourceManager.barricadeHealth<=playerData.getGold())
			{
				totalCost +=(cost*maxHealth/resourceManager.barricadeHealth);
				playerData.addGold (-(cost*maxHealth/resourceManager.barricadeHealth));
				maxHealth += resourceManager.barricadeHealth / 2;

			}
		}	
		setPenalties (health / 5);

	}

	void RemoveTrap(){
		RemoveBT ();
		setPenalties (0);
		resourceManager.allBarricades.Remove (gameObject);
		Destroy (gameObject);
	}

	public void TakeDamage(int damage)
	{
		health -= damage / 100;
		totalCost -= damage / 100;
		setPenalties (health / 5);
	}

}
