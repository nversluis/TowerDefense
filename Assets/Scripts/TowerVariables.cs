using UnityEngine;
using System.Collections;

//Script to initialize all towers, attach to gameObject

public class TowerVariables : MonoBehaviour {

	// initializing constants
	public float maxDistance; //max distance you can put towers
	public GameObject Tower1; // First Tower prefab
	public GameObject Tower2 ; // Second Tower prefab

	public static float Distance; 
	public static GameObject curTower; //current Tower selected
	public static bool hasMagic; //True if player can cast magic, false if not

	// Use this for initialization
	void Start () {
		Distance = maxDistance;
		curTower = null;
		hasMagic = true;
	}
	
	// Update is called once per frame
	void Update () {
		//If 1 pressed, magic weap is selected, cant build towers.
		if (Input.GetKey ("1")) {
			curTower = null;
			hasMagic = true;
			WallScript.DestroyHotSpots();
				}
		//If 2 pressed, building tower will be tower 1, cant cast magic.
		if (Input.GetKey ("2")&&(curTower==null||!curTower.Equals(Tower1))) {
			curTower = Tower1;
			WallScript.DestroyHotSpots();
			hasMagic=false;
				}
		//If 3 pressed, building tower will be tower 2, cant cast magic.
		if (Input.GetKey ("3")&&(curTower==null||!curTower.Equals(Tower2))) {
			curTower = Tower2;
			WallScript.DestroyHotSpots ();
			hasMagic=false;
				}

	}


}
