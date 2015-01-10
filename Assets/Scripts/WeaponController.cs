using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour
{
	private GameObject ResourceManagerObj;
	private ResourceManager resourceManager;
	float planeW;
	PlayerData player = GUIScript.player;
	// initializing constants


	public static int trapGridSize; //The amount of traps you can place in the length of a one planewidth. >0

	private GameObject Tower1;
	// First Tower prefab
	private GameObject Tower2;
	// Second Tower prefab
	private GameObject Tower3;
	private GameObject Tower4;
	private GameObject FloorTower1; //fire
	//First FloorTower prefab
	private GameObject FloorTower2; //poison
	private GameObject FloorTower3; //ice
	private GameObject FloorTower4; //spear
	private GameObject barricade;

	public static GameObject curTower;
	public static GameObject curFloorTower;
	//current Tower selected
	public static int weapon;

	private KeyInputManager inputManager;

	// Use this for initialization
	void Start ()
	{
		ResourceManagerObj = GameObject.Find ("ResourceManager");
		resourceManager = ResourceManagerObj.GetComponent<ResourceManager> ();
		inputManager = GameObject.Find ("KeyInputs").GetComponent<KeyInputManager> ();

		Tower1 = resourceManager.magicTowerHotSpot;
		Tower2 = resourceManager.arrowTowerHotSpot;
		FloorTower1 = resourceManager.fireTrapHotspot;
		FloorTower2 = resourceManager.poisonTrapHotspot;
		FloorTower3 = resourceManager.iceTrapHotspot;
		FloorTower4 = resourceManager.spearTrapHotspot;
		barricade = resourceManager.barricade;
		curTower = null;
		curFloorTower = null;
		weapon = 1;
		trapGridSize = 1;
	}

	// Update is called once per frame
	void Update ()
	{

		//If 1 pressed, magic weap is selected, cant build towers.

		if (Input.GetKey (inputManager.magicInput)&& weapon!=1) {
			curTower = null;
			curFloorTower = null;
			weapon = 1;
			WallScript.DestroyHotSpots ();
			//player.setTower (8);
		}
		//If 2 pressed, building tower will be tower 1, cant cast magic.
		else if (Input.GetKey (inputManager.tow1Input) && (curTower == null || !curTower.Equals (Tower1))&& weapon!=2) {
			curTower = Tower1;
			curFloorTower = null;
			WallScript.DestroyHotSpots ();
			weapon = 2;
			player.setTower (0);
		}
		//If 3 pressed, building tower will be tower 2, cant cast magic.
		else if (Input.GetKey (inputManager.tow2Input) && (curTower == null || !curTower.Equals (Tower2))&& weapon!=3) {
			curTower = Tower2;
			curFloorTower = null;
			WallScript.DestroyHotSpots ();
			weapon = 3; 
			player.setTower (1);
		} else if (Input.GetKey (inputManager.tow3Input)&& weapon!=4 ) {
			curTower = null;
			curFloorTower = FloorTower1;
			WallScript.DestroyHotSpots ();
			weapon = 4; 
			player.setTower (2);
		} else if (Input.GetKey (inputManager.tow4Input)&& weapon!=5) {
			curTower = null;
			curFloorTower = FloorTower2;
			WallScript.DestroyHotSpots ();
			weapon = 5; 
			player.setTower (3);
		} else if (Input.GetKey (inputManager.tow5Input)&& weapon!=6) {
			curTower = null;
			curFloorTower = FloorTower3;
			WallScript.DestroyHotSpots ();
			weapon = 6; 
			player.setTower (4);
		} else if (Input.GetKey (inputManager.tow6Input)&& weapon!=7) {
			curTower = null;
			curFloorTower = FloorTower4;
			WallScript.DestroyHotSpots ();
			weapon = 7; 
			player.setTower (5);
		} else if (Input.GetKey (inputManager.tow7Input)&& weapon!=8) {
			curTower = null;
			curFloorTower = barricade;
			WallScript.DestroyHotSpots ();
			weapon = 8; 
			player.setTower (6);
		}
		else if (Input.GetKey (inputManager.upgradeMenuInput)) {
			curTower = null;
			curFloorTower = null;
			WallScript.DestroyHotSpots ();
			weapon = 50; //arbitrary 
			//player.setTower (7);
		}
	}


}
