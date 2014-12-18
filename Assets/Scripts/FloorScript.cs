using UnityEngine;
using System.Collections;

public class FloorScript : MonoBehaviour {
	private int i;
	private GameObject ResourceManagerObj;
	private ResourceManager resourceManager;
	void Start(){i = 0;
		ResourceManagerObj = GameObject.Find ("ResourceManager");
		resourceManager = ResourceManagerObj.GetComponent<ResourceManager>();
	}
	void Update(){
		//Debug.Log (CameraController.hitObject.name);
		if (gameObject == CameraController.hitObject) { //if the object you are looking at is the floor
			WallScript.DestroyHotSpots (); //Destroy all hotspots
			int aantalPerPlane = WeaponController.trapGridSize;
			GameObject TowerPrefab = WeaponController.curFloorTower;
			if (TowerPrefab != null) { 
				float planeW =resourceManager.planewidth;
				GameObject tower = (GameObject)Instantiate (TowerPrefab, transform.position+new Vector3(0,planeW/1000,0), transform.rotation);
				tower.gameObject.transform.Rotate(270, 0, 0);
				tower.gameObject.transform.localScale=new Vector3( planeW, planeW, planeW)/4.444f;
				tower.renderer.material.color =new Color(0,255,0,0.1f);
				tower.renderer.material.shader = Shader.Find ("Transparent/Diffuse");
				tower.transform.GetChild (0).gameObject.renderer.material.shader = Shader.Find ("Transparent/Diffuse");
				tower.transform.GetChild (0).gameObject.renderer.material.SetColor("_Color", new Color(0,255,0,0.1f));

//				i++;
//				Debug.Log (i);
				/*folowing piece of code is to determine the x and z coordinates of the grid to snap the tower on.
				float planeW = RandomMaze.getPlaneWidth ();
				Vector3 hitpoint = CameraController.hit.point;
				float z = planeW / (2 * aantalPerPlane);
				float Ax = transform.position.x - planeW / 2 + hitpoint.x;
				float xOffset= ((Mathf.Round ((Ax-z) / (2 * z)) * 2 + 1) * z);
				float Az = transform.position.z - planeW / 2 + hitpoint.z;
				float zOffset= ((Mathf.Round ((Az-z) / (2 * z)) * 2 + 1) * z);
				//Debug.Log (transform.position.x - planeW / 2);
				Vector3 loc = new Vector3(-transform.position.x+planeW/2+xOffset,hitpoint.y+planeW/20,planeW/2+zOffset-transform.position.z);
				//Debug.Log (loc);
				GameObject tower = (GameObject)Instantiate (TowerPrefab, loc, transform.rotation);
				tower.gameObject.transform.localScale=new Vector3( planeW, planeW, planeW)/20f*100f;
				*/
			}
		}

	}

}
