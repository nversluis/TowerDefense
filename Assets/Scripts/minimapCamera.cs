using UnityEngine;
using System.Collections;

public class minimapCamera : MonoBehaviour {
	private GameObject player;
	private float heightMinimap;
	private float lengthMinimap;
	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");

		//maxEast = 
	}
	
	// Update is called once per frame
	void Update () {
		float xOffset = player.transform.position.x;
		float yOffset = player.transform.position.z;

		heightMinimap = Screen.width * 0.18f;
		lengthMinimap = heightMinimap;
		//gameObject.camera.orthographicSize = 75 * 16 / 9 * 1000/Screen.width;
		gameObject.camera.pixelRect = new Rect (Screen.width-lengthMinimap, Screen.height-heightMinimap, lengthMinimap, heightMinimap);
		transform.position = new Vector3 (xOffset, 50, yOffset);
		//
		//gameObject.camera.rect = new Rect (0.5f, 0.5f, 0.5f, 0.5f);
	}
}
