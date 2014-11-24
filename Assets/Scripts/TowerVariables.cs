using UnityEngine;
using System.Collections;

public class TowerVariables : MonoBehaviour {


	public float maxDistance;
	public GameObject Tower1;
	public GameObject Tower2 ;

	public static float Distance;
	public static GameObject curTower;

	// Use this for initialization
	void Start () {
		Distance = maxDistance;
		curTower = Tower1;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey ("1")&&!curTower.Equals(Tower1)) {
			curTower = Tower1;
			WallScript.DestroyHotSpots();
				}
		if (Input.GetKey ("2")&&!curTower.Equals(Tower2)) {
			curTower = Tower2;
			WallScript.DestroyHotSpots ();
				}

	}


}
