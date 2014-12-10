using UnityEngine;
using System.Collections;

public class TrapScript : MonoBehaviour {
	//Initizialising
	float MaxDistance=WeaponController.Distance;
	public GameObject realTrap;
	public GameObject redTrap;
	float planeW;
	// Use this for initialization
	void Start () {
		planeW = RandomMaze.getPlaneWidth ();
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
