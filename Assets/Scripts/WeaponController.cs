using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour
{
	private GameObject ResourceManagerObj;
	private ResourceManager resourceManager;
	float planeW;
	// initializing constants


	public static int trapGridSize; //The amount of traps you can place in the length of a one planewidth. >0

	private GameObject Tower1;
	// First Tower prefab
	private GameObject Tower2;
	// Second Tower prefab
	private GameObject FloorTower1;
	//First FloorTower prefab
	private GameObject FloorTower2;


	public static GameObject curTower;
	public static GameObject curFloorTower;
	//current Tower selected
	public static int weapon;

	// Use this for initialization
	void Start ()
	{
		ResourceManagerObj = GameObject.Find ("ResourceManager");
		resourceManager = ResourceManagerObj.GetComponent<ResourceManager> ();

		Tower1 = resourceManager.magicTowerHotSpot;
		Tower2 = resourceManager.tower2;
		FloorTower1 = resourceManager.fireTrapHotspot;
		FloorTower2 = resourceManager.poisonTrapHotspot;
		curTower = null;
		curFloorTower = null;
		weapon = 1;
		trapGridSize = 1;
	}

	// Update is called once per frame
	void Update ()
	{

		//If 1 pressed, magic weap is selected, cant build towers.

		if (Input.GetKey ("1")) {
			curTower = null;
			curFloorTower = null;
			weapon = 1;
			WallScript.DestroyHotSpots ();
		}
		//If 2 pressed, building tower will be tower 1, cant cast magic.
		if (Input.GetKey ("2") && (curTower == null || !curTower.Equals (Tower1))) {
			curTower = Tower1;
			curFloorTower = FloorTower1;
			trapGridSize = 2;
			WallScript.DestroyHotSpots ();
			weapon = 2;
		}
		//If 3 pressed, building tower will be tower 2, cant cast magic.
		if (Input.GetKey ("3") && (curTower == null || !curTower.Equals (Tower2))) {
			curTower = Tower2;
			curFloorTower = FloorTower2;
				trapGridSize=2;
			WallScript.DestroyHotSpots ();
			weapon = 3; 

		}


	}


}
