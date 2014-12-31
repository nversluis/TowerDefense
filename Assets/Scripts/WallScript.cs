﻿using UnityEngine;
using System.Collections;

//Script to attach to the walls. Will instantiate hotspots on walls if player is building towers.

public class WallScript : MonoBehaviour {
			
	Color transparentgreen = new Color(0,255,0,0.1f); //Color of the green hotspot
	Color transparentred = new Color(255,0,0,0.1f); 
	Color curColor;
	string curTag;
	private GameObject ResourceManagerObj;
	private ResourceManager resourceManager;
	float planeW;
	private GameObject player;
	private float maxDistance;
	private int  cost;

	void Start(){
		ResourceManagerObj = GameObject.Find ("ResourceManager");
		resourceManager = ResourceManagerObj.GetComponent<ResourceManager> ();
		planeW = resourceManager.planewidth;
		maxDistance = resourceManager.maxTowerDistance;
		cost = 0;
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
				tower.gameObject.transform.localScale = new Vector3 (1, 1, 1) * planeW*10;
				tower.gameObject.transform.Rotate(new Vector3 (-90, 0, 0));
				tower.gameObject.transform.Rotate(new Vector3 (0, -90, 0));
				tower.gameObject.transform.position += tower.gameObject.transform.forward*planeW/58;
				if (player== null) {
					player = GameObject.Find ("Player");
				}
				if (Vector3.Distance (player.transform.position, transform.position) <= maxDistance) {
					curColor = transparentgreen;
					curTag = "TowerHotSpot";
				} else {
					curColor = transparentred;
					curTag = "HotSpotRed";
				}

				//set cost
				if(TowerPrefab.name.Contains("magic")){
					cost = resourceManager.costMagicTower;
					}

				tower.tag = curTag;

				foreach (Renderer child in tower.GetComponentsInChildren<Renderer>()) {

					child.material.color = curColor;
					child.material.shader = Shader.Find ("Transparent/Diffuse");
				}

//				
                tower.layer = 14;
				tower.transform.parent = gameObject.transform;

			}

			if (Input.GetMouseButtonUp(1) && gameObject.transform.childCount==1) //Sell the tower
			{
				Destroy (gameObject.transform.GetChild(0).gameObject);
				GUIScript.player.addGold (cost / 2);
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
