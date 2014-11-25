using UnityEngine;
using System.Collections;

//Script to attach to the walls. Will instantiate hotspots on walls if player is building towers.

public class WallScript : MonoBehaviour {
			
	Color transparentgreen = new Color(0,255,0,0.1f); //Color of the green hotspot
	
	void Update(){
		if (gameObject == CameraController.getHitObject()) { //if the object you are looking at is the wall
			
			DestroyHotSpots (); //Destroy all objects
		
			GameObject TowerPrefab = TowerVariables.curTower;
			if (TowerPrefab != null) { 
				Vector3 TowerOffset = new Vector3 (Mathf.Sin (transform.eulerAngles.y / 180 * Mathf.PI), 0, Mathf.Cos (transform.eulerAngles.y / 180 * Mathf.PI)) * RandomMaze.getPlaneWidth()/50;
				GameObject tower = (GameObject)Instantiate (TowerPrefab, transform.position + TowerOffset, transform.rotation);
				tower.gameObject.transform.localScale=new Vector3(RandomMaze.getPlaneWidth()/2,RandomMaze.getPlaneWidth()/2,RandomMaze.getPlaneWidth()/100);
				tower.renderer.material.color = transparentgreen;
				tower.renderer.material.shader = Shader.Find ("Transparent/Diffuse");
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
