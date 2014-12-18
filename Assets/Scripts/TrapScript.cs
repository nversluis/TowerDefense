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
	// Use this for initialization
	void Start () {
		ResourceManagerObj = GameObject.Find ("ResourceManager");
		resourceManager = ResourceManagerObj.GetComponent<ResourceManager> ();
		planeW = resourceManager.planewidth;
		if(gameObject.name.Contains("Fire"))
			realTrap=resourceManager.fireTrap;
		if(gameObject.name.Contains("Poison"))
			realTrap=resourceManager.poisonTrap;
		MaxDistance = resourceManager.maxTowerDistance;

	}
	public void BuildTrap(){
		GameObject trap = (GameObject)Instantiate (realTrap, transform.position, Quaternion.identity);//Instantiantion of the tower
		trap.gameObject.transform.localScale = new Vector3 (1, 1, 1)*planeW/20;
		trap.transform.parent = gameObject.transform.parent;
		trap.tag = "Tower";
		WallScript.DestroyHotSpots ();
		trap.SetActiveRecursively (true); 
	}

	void Update(){
		if (Input.GetMouseButtonDown (0)) {
			BuildTrap ();
		}
	}



}
