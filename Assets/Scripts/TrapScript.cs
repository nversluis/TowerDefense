using UnityEngine;
using System.Collections;

public class TrapScript : MonoBehaviour {
	//Initizialising
	float MaxDistance;
	private GameObject realTrap;
	private GameObject redTrap;
	float planeW;
	private GameObject ResourceManagerObj;
	private ResourceManager resourceManager;
	private int cost;
	// Use this for initialization
	void Start () {

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
		}


		MaxDistance = resourceManager.maxTowerDistance;

	}
	public void BuildTrap(){
		if (cost <= resourceManager.gold) {
			GameObject trap = (GameObject)Instantiate (realTrap, transform.position, Quaternion.identity);//Instantiantion of the tower
			trap.gameObject.transform.localScale = new Vector3 (1, 1, 1) * planeW / 20;
			trap.transform.parent = gameObject.transform.parent;
			float randHoek = 90 * Mathf.Floor (Random.value * 4);
			trap.transform.RotateAround (transform.position, Vector3.up, randHoek);
			trap.tag = "Tower";
			WallScript.DestroyHotSpots ();
			trap.SetActiveRecursively (true); 
			resourceManager.gold -= cost;
		} else {
			Debug.Log("Not enough gold to build " + realTrap.name);
		}
	}

	void Update(){
		if (Input.GetMouseButtonDown (0)) {
			BuildTrap ();
		}
	}



}
