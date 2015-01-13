using UnityEngine;
using System.Collections;

public class minimapCamera : MonoBehaviour {
	private GameObject player;
	private GameObject ResourceManagerObj;
	private ResourceManager resourceManager;
	private float heightMinimap;
	private float lengthMinimap;
	private float maxEast;
	private float maxWest;
	private float maxNorth;
	private float maxSouth;
	// Use this for initialization
	void Start () {
		ResourceManagerObj = GameObject.Find ("ResourceManager");
		resourceManager = ResourceManagerObj.GetComponent<ResourceManager> ();
		player = GameObject.Find ("Player");
<<<<<<< .merge_file_a84100
		heightMinimap = resourceManager.heightMinimap;
		lengthMinimap = resourceManager.lengthMinimap;
=======
		//heightMinimap = resourceManager.heightMinimap;
		//lengthMinimap = resourceManager.lengthMinimap;
>>>>>>> .merge_file_a84148
		maxEast = (ResourceManager.mostEast-6)*resourceManager.planewidth;
		maxNorth = (ResourceManager.mostNorth-6)*resourceManager.planewidth;
		maxSouth = (ResourceManager.mostSouth+6)*resourceManager.planewidth;
		maxWest = (ResourceManager.mostWest + 6) * resourceManager.planewidth;
<<<<<<< .merge_file_a84100
=======

>>>>>>> .merge_file_a84148
		//maxEast = 
	}
	
	// Update is called once per frame
	void Update () {
		float xOffset = player.transform.position.x;
		float yOffset = player.transform.position.z;
<<<<<<< .merge_file_a84100
		if (xOffset > maxEast) {
			xOffset = maxEast;
		}
		if (xOffset <= maxWest) {
			xOffset = maxWest;
		}
		if (yOffset > maxNorth) {
			yOffset = maxNorth;
		}
		if (yOffset <= maxSouth) {
			yOffset = maxSouth;
		}

		transform.position = new Vector3 (xOffset, 50, yOffset);
		gameObject.camera.pixelRect = new Rect (Screen.width-lengthMinimap, Screen.height-heightMinimap, lengthMinimap, heightMinimap);
=======
//		if (xOffset > maxEast) {
//			xOffset = maxEast;
//		}
//		if (xOffset <= maxWest) {
//			xOffset = maxWest;
//		}
//		if (yOffset > maxNorth) {
//			yOffset = maxNorth;
//		}
//		if (yOffset <= maxSouth) {
//			yOffset = maxSouth;
//		}

		heightMinimap = Screen.width * 0.18f;
		lengthMinimap = heightMinimap;
		//gameObject.camera.orthographicSize = 75 * 16 / 9 * 1000/Screen.width;
		gameObject.camera.pixelRect = new Rect (Screen.width-lengthMinimap, Screen.height-heightMinimap, lengthMinimap, heightMinimap);
		transform.position = new Vector3 (xOffset, 50, yOffset);
		//
>>>>>>> .merge_file_a84148
		//gameObject.camera.rect = new Rect (0.5f, 0.5f, 0.5f, 0.5f);
	}
}
