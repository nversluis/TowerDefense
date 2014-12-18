using UnityEngine;
using System.Collections;

//Script to attach to the walls. Will instantiate hotspots on walls if player is building towers.

public class WallScript : MonoBehaviour {
			
	Color transparentgreen = new Color(0,255,0,0.1f); //Color of the green hotspot
	private GameObject ResourceManagerObj;
	private ResourceManager resourceManager;
	float planeW;

	void Start(){
		ResourceManagerObj = GameObject.Find ("ResourceManager");
		resourceManager = ResourceManagerObj.GetComponent<ResourceManager> ();
		planeW = resourceManager.planewidth;
	}

	void Update(){
		if (gameObject == CameraController.hitObject) { //if the object you are looking at is the wall
			Vector3 normal = CameraController.hit.normal;
			DestroyHotSpots (); //Destroy all objects
		
			GameObject TowerPrefab = WeaponController.curTower;
			if (TowerPrefab != null &&gameObject.transform.childCount==0) { 
				Vector3 normalToWall = CameraController.hit.normal;
				Vector3 TowerOffset = new Vector3 (Mathf.Sin (transform.eulerAngles.y / 180 * Mathf.PI), 0, Mathf.Cos (transform.eulerAngles.y / 180 * Mathf.PI)) * planeW/50;
				GameObject tower = (GameObject)Instantiate (TowerPrefab, new Vector3(transform.position.x,planeW/2,transform.position.z), transform.rotation);
				tower.gameObject.transform.localScale = new Vector3 (50, 50, 1) * planeW;
				tower.gameObject.transform.Rotate(new Vector3 (-90, 0, 0));
				tower.gameObject.transform.position += tower.gameObject.transform.forward*planeW/58;
				tower.renderer.material.color = transparentgreen;
				tower.renderer.material.shader = Shader.Find ("Transparent/Diffuse");
                tower.layer = 14;
				tower.transform.parent = gameObject.transform;

			}
		}

	}

	//Method to Destroy all hotspots. Called in different scripts.
	public static void DestroyHotSpots(){
		GameObject[] GreenHotspot = GameObject.FindGameObjectsWithTag ("TowerHotSpot");
		GameObject[] RedHotspot = GameObject.FindGameObjectsWithTag ("HotSpotRed");
		foreach (GameObject hotspot in GreenHotspot)
						Destroy (hotspot);
		foreach (GameObject hotspot in RedHotspot)
						Destroy (hotspot);

		}


}
