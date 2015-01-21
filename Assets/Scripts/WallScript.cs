using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//Script to attach to the walls. Will instantiate hotspots on walls if player is building towers.

public class WallScript : MonoBehaviour
{
			
	Color transparentgreen = new Color (0, 255, 0, 0.1f);
	//Color of the green hotspot
	Color transparentred = new Color (255, 0, 0, 0.1f);
	Color curColor;
	string curTag;
	private GameObject ResourceManagerObj;
	private ResourceManager resourceManager;
	float planeW;
	private GameObject player;
	private float maxDistance;
	private int cost;
	private PlayerData playerData;
	private static GameObject resTower;
	private KeyInputManager inputManager;

	void Start ()
	{
		ResourceManagerObj = GameObject.Find ("ResourceManager");
		resourceManager = ResourceManagerObj.GetComponent<ResourceManager> ();
		planeW = resourceManager.planewidth;
		maxDistance = resourceManager.maxTowerDistance;
		playerData = GUIScript.player;
		cost = 0;
		inputManager = GameObject.Find ("KeyInputs").GetComponent<KeyInputManager> ();
	}

	void Update ()
	{

		if (gameObject == CameraController.hitObject) { //if the object you are looking at is the wall
			if (player == null) {
				player = GameObject.Find ("Player");
			}
			//Destroy all objects
			if (gameObject.transform.childCount != 2) {
				DestroyHotSpots ();
			}
			GameObject TowerPrefab = WeaponController.curTower;
			if (TowerPrefab != null && gameObject.transform.childCount == 0 && Vector3.Distance (player.transform.position, transform.position) <= maxDistance) { 
				GameObject tower = (GameObject)Instantiate (TowerPrefab, transform.position, transform.rotation);

				tower.gameObject.transform.localScale = new Vector3 (1, 1, 1) * planeW * 10/2;
				tower.gameObject.transform.Rotate (new Vector3 (-90, 0, 0));
				tower.gameObject.transform.Rotate (new Vector3 (0, -90, 0));
				tower.transform.parent = gameObject.transform;
				//tower.gameObject.transform.position += tower.gameObject.transform.forward * planeW / 58;

				//set costs
				if (TowerPrefab.name.Contains ("magic")) {
                    cost = resourceManager.costMagicTower[ResourceManager.Difficulty];
				} else if (TowerPrefab.name.Contains ("Arrow")) {
                    cost = resourceManager.costArrowTower[ResourceManager.Difficulty];
				}

				if ((Vector3.Distance (player.transform.position, transform.position) <= maxDistance) && cost <= playerData.getGold ()) {
					curColor = transparentgreen;
					curTag = "TowerHotSpot";
				} else {
					curColor = transparentred;
					curTag = "HotSpotRed";
				}

				tower.tag = curTag;
				foreach (Renderer child in tower.GetComponentsInChildren<Renderer>()) 
				{
					child.material.color = curColor;
					child.material.shader = Shader.Find ("Transparent/Diffuse");
				}				
				tower.layer = 14;

			}
			//Sell the tower
			if (WeaponController.weapon == 50) {
				if (gameObject.transform.childCount == 1) {
					GameObject baseOfTower = gameObject.transform.GetChild (0).Find("Base").gameObject;
					resTower = (GameObject)Instantiate (baseOfTower, baseOfTower.transform.position, baseOfTower.transform.rotation);
					resTower.transform.position += resTower.transform.forward/(planeW*100);
					resTower.name = "blueTower";
					resTower.transform.localScale /= 2;
					resTower.gameObject.transform.parent = gameObject.transform;
					resTower.tag = "TowerHotSpot";
					//resTower.transform.GetChild (0).gameObject.SetActive (false);
					Color col;
					int upgradeCost = transform.GetChild(0).gameObject.GetComponent<TowerStats>().upgradeCost;
					
					if(upgradeCost<=playerData.getGold() && transform.GetChild (0).GetComponent<TowerStats> ().level<5){
						col = new Color(0,0,255,0.1f);
					} else
						col = new Color(255,0,0,0.1f);
					foreach (Renderer child in resTower.GetComponentsInChildren<Renderer>()) 
					{
						child.material.color = col;
						child.material.shader = Shader.Find ("Transparent/Diffuse");
					}
				}
				if (gameObject.transform.childCount == 2) {
					if (Input.GetKeyUp (inputManager.upgradeInput)) {
						upgradeTower ();
					}

					//upgrade the tower - todo
					if (Input.GetKeyUp (inputManager.sellInput)) {
						sellTower ();
					}
				}

				
			}

		}

	}

	private void sellTower ()
	{
		GameObject tower = gameObject.transform.GetChild (0).gameObject;
		TowerStats stats = tower.GetComponent<TowerStats> ();
		Destroy (gameObject.transform.GetChild (0).gameObject);
		playerData.addGold (stats.sellCost);
		DestroyHotSpots ();
	}

	private void upgradeTower ()
	{
		GameObject tower = gameObject.transform.GetChild (0).gameObject;
		TowerStats stats = tower.GetComponent<TowerStats> ();
		if (stats.level < 5) {
			if (stats.upgradeCost <= playerData.getGold ()) {
				stats.level++;
				stats.attack += (int)stats.attackUpgrade;
				stats.speed += stats.speedUpgrade;
				stats.attackUpgrade = stats.attack / 2;
				playerData.addGold (-stats.upgradeCost);
				stats.sellCost *= 2;
				stats.upgradeCost *= 2;
			} else {
				GameObject.Find ("GUIMain").GetComponent<GUIScript> ().Notification ("NoGold");
			}
		}
		WallScript.DestroyHotSpots();
	}



	//Method to Destroy all hotspots. Called in different scripts.
	public static void DestroyHotSpots ()
	{
		GameObject[] GreenHotspot = GameObject.FindGameObjectsWithTag ("TowerHotSpot");
		GameObject[] RedHotspot = GameObject.FindGameObjectsWithTag ("HotSpotRed");
		foreach (GameObject hotspot in GreenHotspot)
			Destroy (hotspot);
		foreach (GameObject hotspot in RedHotspot)
			Destroy (hotspot);

	}


}
