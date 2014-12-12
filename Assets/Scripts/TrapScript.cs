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
		trap.gameObject.transform.localScale = transform.localScale/4.444f;
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
