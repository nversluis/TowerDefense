using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour
{

	// initializing constants
	public float maxDistance;
	//max distance you can put towers
	public GameObject Tower1;
	// First Tower prefab
	public GameObject Tower2;
	// Second Tower prefab

	public static float Distance;
	public static GameObject curTower;
	//current Tower selected
	public static int weapon;

	// Use this for initialization
	void Start ()
	{
		Distance = maxDistance;
		curTower = null;
		weapon = 1;
	}

	// Update is called once per frame
	void Update ()
	{

		//If 1 pressed, magic weap is selected, cant build towers.

		if (Input.GetKey ("1")) {
			curTower = null;
			weapon = 1;
			WallScript.DestroyHotSpots ();
		}
		//If 2 pressed, building tower will be tower 1, cant cast magic.
		if (Input.GetKey ("2") && (curTower == null || !curTower.Equals (Tower1))) {
			curTower = Tower1;
			WallScript.DestroyHotSpots ();
			weapon = 2;
		}
		//If 3 pressed, building tower will be tower 2, cant cast magic.
		if (Input.GetKey ("3") && (curTower == null || !curTower.Equals (Tower2))) {
			curTower = Tower2;
			WallScript.DestroyHotSpots ();
			weapon = 3; 
		}


	}


}
