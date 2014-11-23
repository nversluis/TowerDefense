using UnityEngine;
using System.Collections;

public class TowerVariables : MonoBehaviour {


	public float maxDistance;
	public GameObject Tower1;
	public GameObject Tower2 ;

	public static float Distance;
	public static GameObject curTower;
	public static bool hasMagic;

	// Use this for initialization
	void Start () {
		Distance = maxDistance;
		curTower = null;
		hasMagic = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey ("1")) {
			curTower = null;
			hasMagic = true;
				}

		if (Input.GetKey ("2")&&(curTower==null||!curTower.Equals(Tower1))) {
			curTower = Tower1;
			WallScript.DestroyHotSpots();
			hasMagic=false;
				}
		if (Input.GetKey ("3")&&(curTower==null||!curTower.Equals(Tower2))) {
			curTower = Tower2;
			WallScript.DestroyHotSpots ();
			hasMagic=false;
				}

	}


}
