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
		heightMinimap = resourceManager.heightMinimap;
		lengthMinimap = resourceManager.lengthMinimap;
		maxEast = (ResourceManager.mostEast-6)*resourceManager.planewidth;
		maxNorth = (ResourceManager.mostNorth-6)*resourceManager.planewidth;
		maxSouth = (ResourceManager.mostSouth+6)*resourceManager.planewidth;
		maxWest = (ResourceManager.mostWest + 6) * resourceManager.planewidth;

		//maxEast = 
	}
	
	// Update is called once per frame
	void Update () {
		float xOffset = player.transform.position.x;
		float yOffset = player.transform.position.z;
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

		transform.position = new Vector3 (xOffset, 50, yOffset);
		//gameObject.camera.pixelRect = new Rect (Screen.width-lengthMinimap, Screen.height-heightMinimap, lengthMinimap, heightMinimap);
		//gameObject.camera.rect = new Rect (0.5f, 0.5f, 0.5f, 0.5f);
	}
}
