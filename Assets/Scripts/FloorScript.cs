﻿using UnityEngine;
using System.Collections;

public class FloorScript : MonoBehaviour
{

	Color transparentgreen = new Color (0, 255, 0, 0.1f);
	Color transparentred = new Color (255, 0, 0, 0.1f);
	private int i;
	private GameObject ResourceManagerObj;
	private ResourceManager resourceManager;
	private PlayerData playerData = GUIScript.player;
	private int cost;
	private KeyInputManager inputManager;

	void Start ()
	{
		i = 0;
		ResourceManagerObj = GameObject.Find ("ResourceManager");
		resourceManager = ResourceManagerObj.GetComponent<ResourceManager> ();
		cost = 0;
		inputManager = GameObject.Find ("KeyInputs").GetComponent<KeyInputManager> ();
	}

	void Update ()
	{
		//Debug.Log (CameraController.hitObject.name);
		if (gameObject == CameraController.hitObject) { //if the object you are looking at is the floor

			if (gameObject.transform.childCount != 3) {
				WallScript.DestroyHotSpots ();//Destroy all hotspots
			}
			GameObject TowerPrefab = WeaponController.curFloorTower;
			if (TowerPrefab != null && gameObject.transform.childCount == 1) { 
				float planeW = resourceManager.planewidth;
				GameObject tower = (GameObject)Instantiate (TowerPrefab, transform.position + new Vector3 (0, planeW / 1000, 0), transform.rotation);
				//tower.gameObject.transform.Rotate(270, 0, 0);
				tower.gameObject.transform.localScale = new Vector3 (1, 1, 1) * planeW * 5;

				float randHoek = 90 * Mathf.Floor (Random.value * 4);
				tower.transform.RotateAround (transform.position, Vector3.up, randHoek);

				//tower.renderer.material.color =new Color(0,255,0,0.1f);
				//tower.renderer.material.shader = Shader.Find ("Transparent/Diffuse");
				foreach (Renderer child in tower.GetComponentsInChildren<Renderer>()) {
					foreach (Material mat in child.materials) {
						mat.shader = Shader.Find ("Transparent/Diffuse");
						if (cost >= playerData.getGold ()) {
							mat.color = transparentred;
						} else {
							mat.color = transparentgreen;
						}
					}
				}

				if (TowerPrefab.name.Contains ("Fire")) {
					cost = resourceManager.costFireTrap;
				} else if (TowerPrefab.name.Contains ("Poison")) {
					cost = resourceManager.costPoisonTrap;
				} else if (TowerPrefab.name.Contains ("Ice")) {
					cost = resourceManager.costIceTrap;
				} else if (TowerPrefab.name.Contains ("Spear")) {
					//cost = resourceManager.costFireTrap;
				}

				//tower.transform.GetChild (0).gameObject.renderer.material.shader = Shader.Find ("Transparent/Diffuse");
				//tower.transform.GetChild (0).gameObject.renderer.
				tower.transform.parent = gameObject.transform;			
			}
			//Sell the trap
			if (WeaponController.weapon == 50) {
				if (gameObject.transform.childCount == 2) { 
					GameObject resTower = (GameObject)Instantiate (gameObject.transform.GetChild (1).gameObject, transform.position + new Vector3 (0, resourceManager.planewidth / 900, 0), transform.rotation);
					resTower.gameObject.transform.localScale = new Vector3 (1, 1, 1) * 1.1f / 2;
					resTower.name = "blueTower";
					resTower.gameObject.transform.parent = gameObject.transform;
					resTower.tag = "TowerHotSpot";
					resTower.transform.GetComponent<BoxCollider> ().enabled = false;
					foreach (Renderer child in resTower.GetComponentsInChildren<Renderer>()) {
						child.material.color = new Color (0, 0, 255, 0.1f);
						child.material.shader = Shader.Find ("Transparent/Diffuse");
					}
				}
				if (Input.GetKeyUp (inputManager.upgradeInput)) {
					upgradeTower ();
				}
				//upgrade the tower - todo
				if (Input.GetKeyUp (inputManager.sellInput)) {
					sellTower ();
				}

				//bring up the menu to show ot sell or upgrade tower (with costs) and upgrade stats
				//if (Input.GetMouseButtonDown (1)) {
				//	showMenu ();
				//}

				//if (Input.GetKeyUp (KeyCode.C)) {
				//	GameObject.Find ("GUIMain").GetComponent<GUIScript> ().TowerPopup.SetActive (false);
				//}

			}


		}

	}

	private void sellTower ()
	{

		Destroy (gameObject.transform.GetChild (1).gameObject);
		GUIScript.player.addGold (cost / 2);
		GameObject.Find ("TowerPopup").SetActive (false);
		WallScript.DestroyHotSpots();
	}

	private void upgradeTower ()
	{
		Debug.Log ("Not yet implemented");
		GameObject.Find ("GUIMain").GetComponent<GUIScript> ().TowerPopup.SetActive (false);
		WallScript.DestroyHotSpots();
	}

	private void showMenu ()
	{
		GameObject popUpPanel = GameObject.Find ("GUIMain").GetComponent<GUIScript> ().getPopUpPanel ();
		popUpPanel.SetActive (true);

	}
}

