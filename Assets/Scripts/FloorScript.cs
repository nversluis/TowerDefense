using UnityEngine;
using System.Collections;

public class FloorScript : MonoBehaviour {

	Color transparentgreen = new Color(0,255,0,0.1f); //Color of the green hotspot

	void Update(){
		if (gameObject == CameraController.hitObject) { //if the object you are looking at is the wall
			WallScript.DestroyHotSpots (); //Destroy all objects
			int aantalPerPlane = WeaponController.trapGridSize;
			GameObject TowerPrefab = WeaponController.curFloorTower;
			if (TowerPrefab != null) { 

				//folowing piece of code is to determine the x and z coordinates of the grid to snap the tower on.
				float planeW = RandomMaze.getPlaneWidth ();
				Vector3 hitpoint = CameraController.hit.point;
				float z = planeW / (2 * aantalPerPlane);
				float Ax = transform.position.x - planeW / 2 + hitpoint.x;
				float xOffset= ((Mathf.Round ((Ax-z) / (2 * z)) * 2 + 1) * z);
				float Az = transform.position.z - planeW / 2 + hitpoint.z;
				float zOffset= ((Mathf.Round ((Az-z) / (2 * z)) * 2 + 1) * z);
				Debug.Log (transform.position.x-planeW/2-xOffset);
				//Debug.Log (transform.position.x - planeW / 2);
				Vector3 loc = new Vector3(-transform.position.x+planeW/2+xOffset,hitpoint.y,planeW/2+zOffset-transform.position.z);
				//Debug.Log (loc);
				GameObject tower = (GameObject)Instantiate (TowerPrefab, loc, transform.rotation);
				tower.gameObject.transform.localScale=new Vector3( planeW/4, planeW/100, planeW/4)*100;
				tower.renderer.material.color = transparentgreen;
				tower.renderer.material.shader = Shader.Find ("Transparent/Diffuse");
			}
		}

	}

}
