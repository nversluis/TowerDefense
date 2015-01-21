using UnityEngine;
using System.Collections;

public class MinimapItem : MonoBehaviour {

	GameObject player;
	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (player == null) {
			player = GameObject.Find ("Player");
		}
		else if ((player.transform.position - transform.position).magnitude >= 70) {
			gameObject.layer = 16;
		} else {
			gameObject.layer = 9;
		}
	
	}
}
