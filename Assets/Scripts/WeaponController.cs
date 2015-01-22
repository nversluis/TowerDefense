using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour
{
	private GameObject ResourceManagerObj;
	private ResourceManager resourceManager;
	float planeW;
	PlayerData player = GUIScript.player;

	private bool weapSelected;
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
	private GameObject barricade;

	public static GameObject curTower;
	public static GameObject curFloorTower;
	//current Tower selected
	public static int weapon;

	private KeyInputManager inputManager;
    int prevTower;
    int prevWeap;
    int prevSkill;
    int prevSetTower;

    float weaponscroller;
    float weaponscrollerTop;
    float weaponscrollerDown;
    float scrollspeed = 10;
    float scrollspeedtower = 10;

    float towerscroller;
    float towerscrollerTop;
    float towerscrollerDown;


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
		barricade = resourceManager.barricade;
        weapSelected = true;
        curTower = null;
        curFloorTower = null;
        weapon = 1;
        player.setSkill(0);
        WallScript.DestroyHotSpots();
		trapGridSize = 1;
        weaponscroller = 0;
        weaponscrollerTop = 3;
        weaponscrollerDown = 0;
        towerscroller = 0;
        towerscrollerTop = 1;
        towerscrollerDown = 0;
        prevWeap = 1;
        prevTower = 25;
        

	}

	// Update is called once per frame
	void Update ()
	{
        if (!Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (weapSelected)
                {
                    weapSelected = false;
                    curTower = Tower1;
                    curFloorTower = null;
                    WallScript.DestroyHotSpots();
                    prevWeap = weapon;
                    prevSkill = player.getSkill();
                    weapon = prevTower;
                    player.setTower(prevSetTower);


                }
                else
                {
                    weapSelected = true;
                    curTower = null;
                    curFloorTower = null;
                    prevTower = weapon;
                    prevSetTower = player.getTower();
                    weapon = prevWeap;
                    player.setSkill(prevSkill);
                    WallScript.DestroyHotSpots();
                }
            }
            //If 1 pressed, magic weap is selected, cant build towers.
            if ((Input.GetKey(inputManager.sword1Input) || (int)Mathf.Round(weapon + weaponscroller) == 1) && weapon != 1 && weapSelected)
            {
                curTower = null;
                curFloorTower = null;
                weapon = 1;
                player.setSkill(0);
                WallScript.DestroyHotSpots();
                //player.setTower (8);
                weaponscroller = 0;
                weaponscrollerTop = 1;
                weaponscrollerDown = 0;

            }
            if ((Input.GetKey(inputManager.sword2Input) || (int)Mathf.Round(weapon + weaponscroller) == 2) && weapon != 2 && weapSelected)
            {
                curTower = null;
                curFloorTower = null;
                weapon = 2;
                player.setSkill(1);
                WallScript.DestroyHotSpots();
                //player.setTower (8);
                weaponscroller = 0;
                weaponscrollerTop = 1;
                weaponscrollerDown = -1;

            }
            if ((Input.GetKey(inputManager.magic1Input) || (int)Mathf.Round(weapon + weaponscroller) == 3) && weapon != 3 && weapSelected)
            {
                curTower = null;
                curFloorTower = null;
                weapon = 3;
                player.setSkill(2);
                WallScript.DestroyHotSpots();
                //player.setTower (8);
                weaponscroller = 0;

                weaponscrollerTop = 1;
                weaponscrollerDown = -1;
            }
            if ((Input.GetKey(inputManager.magic2Input) || (int)Mathf.Round(weapon + weaponscroller) == 4) && weapon != 4 && weapSelected)
            {
                curTower = null;
                curFloorTower = null;
                weapon = 4;
                player.setSkill(3);
                WallScript.DestroyHotSpots();
                //player.setTower (8);
                weaponscroller = 0;

                weaponscrollerTop = 0;
                weaponscrollerDown = -1;

            }

            //If 2 pressed, building tower will be tower 1, cant cast magic.
            else if ((Input.GetKey(inputManager.tow1Input) || (int)Mathf.Round(weapon + towerscroller) == 34) && weapon != 25 && !weapSelected)
            {
                curTower = Tower1;
                curFloorTower = null;
                WallScript.DestroyHotSpots();
                weapon = 25;
                player.setTower(0);
                towerscroller = 0;
                towerscrollerTop = 1;
                towerscrollerDown = 0;

            }
            //If 3 pressed, building tower will be tower 2, cant cast magic.
            else if ((Input.GetKey(inputManager.tow2Input) || ((int)Mathf.Round(weapon + towerscroller) == 26 || (int)Mathf.Round(weapon + towerscroller) == 44)) && weapon != 35 && !weapSelected)
            {
                curTower = Tower2;
                curFloorTower = null;
                WallScript.DestroyHotSpots();
                weapon = 35;
                player.setTower(1);
                towerscroller = 0;
                towerscrollerTop = 1;
                towerscrollerDown = -1;
            }
            else if ((Input.GetKey(inputManager.tow3Input) || ((int)Mathf.Round(weapon + towerscroller) == 36 || (int)Mathf.Round(weapon + towerscroller) == 54)) && weapon != 45 && !weapSelected)
            {
                curTower = null;
                curFloorTower = FloorTower1;
                WallScript.DestroyHotSpots();
                weapon = 45;
                player.setTower(2);
                towerscroller = 0;
                towerscrollerTop = 1;
                towerscrollerDown = -1;
            }
            else if ((Input.GetKey(inputManager.tow4Input) || ((int)Mathf.Round(weapon + towerscroller) == 46 || (int)Mathf.Round(weapon + towerscroller) == 64)) && weapon != 55 && !weapSelected)
            {
                curTower = null;
                curFloorTower = FloorTower2;
                WallScript.DestroyHotSpots();
                weapon = 55;
                player.setTower(3);
                towerscroller = 0;
                towerscrollerTop = 1;
                towerscrollerDown = -1;
            }
            else if ((Input.GetKey(inputManager.tow5Input) || ((int)Mathf.Round(weapon + towerscroller) == 56 || (int)Mathf.Round(weapon + towerscroller) == 84)) && weapon != 65 && !weapSelected)
            {
                curTower = null;
                curFloorTower = FloorTower3;
                WallScript.DestroyHotSpots();
                weapon = 65;
                player.setTower(4);
                towerscroller = 0;
                towerscrollerTop = 1;
                towerscrollerDown = -1;
                //} else if (Input.GetKey (inputManager.tow6Input)&& weapon!=75 && !weapSelected) {
                //    curTower = null;
                //    curFloorTower = FloorTower4;
                //    WallScript.DestroyHotSpots ();
                //    weapon = 75; 
                //    player.setTower (5);
            }
            else if ((Input.GetKey(inputManager.tow7Input) || ((int)Mathf.Round(weapon + towerscroller) == 66 || (int)Mathf.Round(weapon + towerscroller) == 49)) && weapon != 85 && !weapSelected)
            {
                curTower = null;
                curFloorTower = barricade;
                WallScript.DestroyHotSpots();
                weapon = 85;
                player.setTower(5);
                towerscroller = 0;
                towerscrollerTop = 1;
                towerscrollerDown = -1;
            }

            else if ((Input.GetKey(inputManager.upgradeMenuInput) || (int)Mathf.Round(weapon + towerscroller) == 86) && weapon != 50 && !weapSelected)
            {
                curTower = null;
                curFloorTower = null;
                WallScript.DestroyHotSpots();
                weapon = 50; //arbitrary 
                player.setTower(6);
                towerscroller = 0;
                towerscrollerTop = 0;
                towerscrollerDown = -1;


            }
            if (weapSelected)
            {
                weaponscroller = Mathf.Clamp(weaponscroller + Input.GetAxisRaw("Mouse ScrollWheel") * scrollspeed, weaponscrollerDown, weaponscrollerTop);
            }
            else
            {
                towerscroller = Mathf.Clamp(weaponscroller + Input.GetAxisRaw("Mouse ScrollWheel") * scrollspeedtower, towerscrollerDown, towerscrollerTop);
            }

        }
	}


}
