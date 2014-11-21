using UnityEngine;
using System.Collections;

public class Tower1 : TowerScript {

	public GameObject TowerBullet;

	void OnCollisionStay(Collision col){
		col.gameObject.transform.Rotate(new Vector3(0, 180,0));
		GameObject bullet = (GameObject)Instantiate (TowerBullet, transform.position, Quaternion.identity);
		}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		BaseUpdate ();
	}
}
