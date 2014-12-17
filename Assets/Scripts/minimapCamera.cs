using UnityEngine;
using System.Collections;

public class minimapCamera : MonoBehaviour {
	private GameObject player;
	private GameObject ResourceManagerObj;
	private ResourceManager resourceManager;
	// Use this for initialization
	void Start () {
		ResourceManagerObj = GameObject.Find ("ResourceManager");
		resourceManager = ResourceManagerObj.GetComponent<ResourceManager> ();
		player = GameObject.Find ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = player.transform.position + new Vector3 (0, 4, 0);
	}
}
