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
			Vector3 normal = CameraController.hit.normal;
			//Destroy all objects
			if (gameObject.transform.childCount != 2) {
				DestroyHotSpots ();
			}
			GameObject TowerPrefab = WeaponController.curTower;
			if (TowerPrefab != null && gameObject.transform.childCount == 0) { 
				Vector3 normalToWall = CameraController.hit.normal;
				Vector3 TowerOffset = new Vector3 (Mathf.Sin (transform.eulerAngles.y / 180 * Mathf.PI), 0, Mathf.Cos (transform.eulerAngles.y / 180 * Mathf.PI)) * planeW / 50;
				GameObject tower = (GameObject)Instantiate (TowerPrefab, new Vector3 (transform.position.x, planeW / 2, transform.position.z), transform.rotation);
				tower.gameObject.transform.localScale = new Vector3 (1, 1, 1) * planeW * 10;
				tower.gameObject.transform.Rotate (new Vector3 (-90, 0, 0));
				tower.gameObject.transform.Rotate (new Vector3 (0, -90, 0));
				//tower.gameObject.transform.position += tower.gameObject.transform.forward * planeW / 58;
				if (player == null) {
					player = GameObject.Find ("Player");
				}
				//set costs
				if (TowerPrefab.name.Contains ("magic")) {
					cost = resourceManager.costMagicTower;
				} else if (TowerPrefab.name.Contains ("Arrow")) {
					cost = resourceManager.costArrowTower;
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
				tower.transform.parent = gameObject.transform;
			}
			//Sell the tower
			if (WeaponController.weapon == 50) {
				if (gameObject.transform.childCount == 1) {
					GameObject baseOfTower = gameObject.transform.GetChild (0).Find("Base").gameObject;
					resTower = (GameObject)Instantiate (baseOfTower, baseOfTower.transform.position, baseOfTower.transform.rotation);
					resTower.transform.position += resTower.transform.forward/(planeW*100);
					resTower.name = "blueTower";
					resTower.gameObject.transform.parent = gameObject.transform;
					resTower.tag = "TowerHotSpot";
					//resTower.transform.GetChild (0).gameObject.SetActive (false);
					foreach (Renderer child in resTower.GetComponentsInChildren<Renderer>()) 
					{
						child.material.color = new Color (0, 0, 255, 0.1f);
						child.material.shader = Shader.Find ("Transparent/Diffuse");
					}
				}
				if (Input.GetKeyUp (inputManager.upgradeInput)) {
					upgradeTower ();
				}

				//upgrade the tower - todo
				if (Input.GetKeyUp (inputManager.sellInput)) {
					sellTower ();
				}

				//bring up the menu to show ot sell or upgrade tower (with costs) and upgrade stats
				if (Input.GetMouseButtonDown (1)) {
					//	showMenu ();
				}

				if (Input.GetKeyUp (KeyCode.C)) {
					GameObject.Find ("GUIMain").GetComponent<GUIScript> ().getPopUpPanel ().SetActive (false);
				}
			}

		}

	}

	private void sellTower ()
	{
		DestroyHotSpots ();
		Destroy (gameObject.transform.GetChild (0).gameObject);
		GUIScript.player.addGold (cost / 2);
		GameObject.Find ("GUIMain").GetComponent<GUIScript> ().getPopUpPanel ().SetActive (false);
	}

	private void upgradeTower ()
	{
		Debug.Log ("Not yet implemented");
		GameObject.Find ("GUIMain").GetComponent<GUIScript> ().getPopUpPanel ().SetActive (false);
	}

	private void showMenu ()
	{
		GameObject popUpPanel = GameObject.Find ("GUIMain").GetComponent<GUIScript> ().getPopUpPanel ();
		popUpPanel.SetActive (true);
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
