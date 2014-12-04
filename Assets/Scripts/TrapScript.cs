using UnityEngine;
using System.Collections;

public class TrapScript : MonoBehaviour {
	//Initizialising
	float MaxDistance=WeaponController.Distance;
	public GameObject realTrap;
	public GameObject redTrap;

	public void BuildTrap(){
		GameObject trap = (GameObject)Instantiate (realTrap, transform.position, transform.rotation);//Instantiantion of the tower
		trap.gameObject.transform.localScale = transform.localScale/100;
		trap.tag = "Tower";
		WallScript.DestroyHotSpots ();
		trap.SetActiveRecursively (true); 
	}

	void Update(){
		if (Input.GetMouseButtonUp (0)) {
			Debug.Log (4);
			BuildTrap ();
		}
	}

	// Use this for initialization
	void Start () {
	
	}

}
