using UnityEngine;
using System.Collections;

public class TrapScript : MonoBehaviour
{
	//Initizialising
	float MaxDistance;
	private GameObject realTrap;
	private GameObject redTrap;
	float planeW;
	private GameObject ResourceManagerObj;
	private ResourceManager resourceManager;
	private int cost;
	private PlayerData playerData = GUIScript.player;
	Color transparentgreen = new Color(0, 255, 0, 0.1f); //Color of the green prefab
	Color transparentred = new Color(255, 0, 0, 0.1f); //Color of the red prefab
	// Use this for initialization
	void Start ()
	{

		ResourceManagerObj = GameObject.Find ("ResourceManager");
		resourceManager = ResourceManagerObj.GetComponent<ResourceManager> ();
		planeW = resourceManager.planewidth;
		cost = 9000;
		if (gameObject.name.Contains ("Fire")) {
			realTrap = resourceManager.fireTrap;
			cost = resourceManager.costFireTrap;
		} else if (gameObject.name.Contains ("Poison")) {
			realTrap = resourceManager.poisonTrap;
			cost = resourceManager.costPoisonTrap;
		} else if (gameObject.name.Contains ("Ice")) {
			realTrap = resourceManager.iceTrap;
			cost = resourceManager.costIceTrap;
		} else if (gameObject.name.Contains ("Spear")) {
			realTrap = resourceManager.spearTrap;
			//cost = resourceManager.costFireTrap;
		} else if (gameObject.name.Contains ("arricade")) {
			realTrap = resourceManager.barricade;
			cost = resourceManager.costBarricade;
			//cost = resourceManager.costFireTrap;
		} 


		MaxDistance = resourceManager.maxTowerDistance;

	}

	public void BuildTrap ()
	{
		if (cost <= playerData.getGold () && !transform.parent.gameObject.GetComponent<FloorScript>().hasEnemy) {
			GameObject trap = (GameObject)Instantiate (realTrap, transform.position, Quaternion.identity);//Instantiantion of the tower
			trap.gameObject.transform.localScale = new Vector3 (1, 1, 1) * planeW / 20;
			trap.transform.parent = gameObject.transform.parent;
			float randHoek = 90 * Mathf.Floor (Random.value * 4);
			trap.transform.RotateAround (transform.position, Vector3.up, randHoek);
			trap.tag = "Tower";
			WallScript.DestroyHotSpots ();
			trap.SetActiveRecursively (true); 
			playerData.addGold (-cost);
			TowerStats stats = trap.GetComponent<TowerStats> ();
			stats.upgradeCost = cost;
			stats.sellCost = cost/2;
		} else {

		}
	}

	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) {
			BuildTrap ();
		}
		if (cost > playerData.getGold () || transform.parent.gameObject.GetComponent<FloorScript>().hasEnemy) {
			foreach (Renderer child in gameObject.GetComponentsInChildren<Renderer>()) {
				foreach (Material mat in child.materials) {
					mat.shader = Shader.Find ("Transparent/Diffuse");
					mat.color = transparentred;
				}
			} 

		} else 
			foreach (Renderer child in gameObject.GetComponentsInChildren<Renderer>()) {
				foreach (Material mat in child.materials) {
					mat.shader = Shader.Find ("Transparent/Diffuse");
					mat.color = transparentgreen;
				}
			}
	}



}
