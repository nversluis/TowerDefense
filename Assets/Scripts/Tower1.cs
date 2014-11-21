using UnityEngine;
using System.Collections;

public class Tower1 : TowerScript {

	void OnCollisionStay(Collision col){
		col.gameObject.transform.Rotate(new Vector3(0, 180,0));
		}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		BaseUpdate ();
	}
}
